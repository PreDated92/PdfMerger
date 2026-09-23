using System.Collections;
using PdfMerger.Core;

namespace PdfMerger
{
    public partial class Form1 : Form
    {
        private readonly PdfMergeService _mergeService = new();
        private int _sortColumn = -1;
        private SortOrder _sortOrder = SortOrder.None;

        private const int PreviewExpandedWidth = 280;
        private const int PreviewCollapsedWidth = 32;

        private const string ListSortingDevDocsText = """
            List Sorting — How It Works
            ============================

            The file list (_listFiles) is a System.Windows.Forms.ListView in Details
            view with two columns: Name and Date Modified.

            Clicking a column header:
            - Raises ListView.ColumnClick, handled by ListFiles_ColumnClick.
            - Clicking a new column sets _sortColumn to that column and _sortOrder to
              Ascending.
            - Clicking the same column again toggles _sortOrder between Ascending and
              Descending. Clicking the other column restarts it at Ascending.
            - The handler assigns a new PdfFileItemComparer(_sortColumn, _sortOrder) to
              _listFiles.ListViewItemSorter, then calls _listFiles.Sort() to apply it.

            How rows are compared (PdfFileItemComparer, nested in Form1.cs):
            - Name column (0): string.Compare on ListViewItem.Text, case-insensitive,
              culture-aware.
            - Date Modified column (1): DateTime.Compare using File.GetLastWriteTime of
              the full path stored in each item's Tag.
            - The comparison result is negated when _sortOrder is Descending.

            Sort indicator:
            - UpdateSortIndicators appends " ▲" (ascending) or " ▼" (descending) to the
              active column's header text. The inactive header stays plain.

            Sorting is one-shot, not a maintained live order:
            - ClearSort resets _sortColumn/_sortOrder to their defaults, clears
              ListViewItemSorter, and removes both header arrows.
            - It's called from AddFiles, BtnRemove_Click, MoveSelectedItem, and
              BtnClear_Click - every action that mutates the list - because the list
              isn't guaranteed to still be sorted after a mutation.
            """;

        private const string PdfPreviewDevDocsText = """
            PDF Preview — How It Works
            ============================

            "Show Preview" (_chkShowPreview, in the button column next to the file
            list) turns the whole feature on or off:
            - Unchecked (default): _previewSplit.Panel2Collapsed is true, so the
              preview panel and its splitter are entirely hidden. The file list uses
              the full tab width, exactly as it did before this feature existed.
            - Checked: _previewSplit.Panel2Collapsed becomes false, the panel appears
              at its expanded width, and UpdatePreview() immediately shows the
              current _listFiles selection.

            Collapsing the panel itself is independent of that checkbox:
            - _previewPanel is a standalone PdfPreviewPanel UserControl (see
              PdfPreviewPanel.cs). It knows nothing about Form1, _listFiles, or
              SplitContainer - its public surface is ShowPreviewAsync(path),
              ClearPreview(), IsCollapsed/SetCollapsed, and a CollapsedChanged event.
            - Its header has a small toggle button that flips IsCollapsed and raises
              CollapsedChanged. Form1 handles that event (PreviewPanel_CollapsedChanged)
              by resizing _previewSplit.SplitterDistance between
              PreviewCollapsedWidth (32px, just the header) and
              PreviewExpandedWidth (280px) - the panel never resizes itself, since it
              has no reference to the SplitContainer hosting it.
            - _previewSplit.FixedPanel is set to Panel2, so the preview keeps a
              constant pixel width when the window is resized (the file list absorbs
              the change) without any extra resize handling - the same idea already
              used for the Date Modified column width in ListFiles_Resize.

            Rendering:
            - PdfPreviewPanel hosts a Microsoft.Web.WebView2.WinForms.WebView2
              control, reusing Edge's built-in PDF viewer instead of a custom
              renderer.
            - The WebView2 core is only initialized (EnsureCoreWebView2Async) the
              first time a file is actually previewed, not at startup, so users who
              never enable preview never pay that cost.
            - ShowPreviewAsync navigates to the file's file:// URI
              (new Uri(path).AbsoluteUri). If EnsureCoreWebView2Async throws (for
              example the WebView2 Runtime is missing), PdfPreviewPanel catches the
              exception and shows a plain-text message in place of the browser
              control instead of throwing into Form1.

            Selection tracking:
            - ListFiles_SelectedIndexChanged calls the shared UpdatePreview() helper,
              which also runs from every place UpdateStatus() already runs
              (AddFiles, BtnRemove_Click, BtnClear_Click) so adding, removing, or
              clearing files keeps the preview in sync.
            - UpdatePreview() does nothing while the checkbox is unchecked. With a
              selection, it previews the first selected item
              (_listFiles.SelectedItems[0]); with no selection, it calls
              ClearPreview() to show the "No file selected" placeholder.
            - ShowPreviewAsync calls from UpdatePreview() are fire-and-forget. Rapid
              selection changes can in theory complete out of order and briefly show
              a stale page - this is accepted as a minor, self-correcting cosmetic
              issue rather than adding cancellation-token plumbing to a
              preview-only feature.
            """;

        public Form1()
        {
            InitializeComponent();
            ListFiles_Resize(_listFiles, EventArgs.Empty);
            _txtDevDocsListSorting.Text = ListSortingDevDocsText;
            _txtDevDocsPdfPreview.Text = PdfPreviewDevDocsText;
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            if (_openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                AddFiles(_openFileDialog.FileNames);
            }
        }

        private void BtnRemove_Click(object? sender, EventArgs e)
        {
            // Remove from the bottom up so indices stay valid during removal.
            for (int i = _listFiles.SelectedIndices.Count - 1; i >= 0; i--)
            {
                _listFiles.Items.RemoveAt(_listFiles.SelectedIndices[i]);
            }

            ClearSort();
            UpdateStatus();
            UpdatePreview();
        }

        private void BtnMoveUp_Click(object? sender, EventArgs e)
        {
            MoveSelectedItem(-1);
        }

        private void BtnMoveDown_Click(object? sender, EventArgs e)
        {
            MoveSelectedItem(1);
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            _listFiles.Items.Clear();
            ClearSort();
            UpdateStatus();
            UpdatePreview();
        }

        private void ListFiles_DragEnter(object? sender, DragEventArgs e)
        {
            if (e.Data is not null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void ListFiles_DragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data?.GetData(DataFormats.FileDrop) is string[] paths)
            {
                AddFiles(paths);
            }
        }

        private async void BtnMerge_Click(object? sender, EventArgs e)
        {
            if (_listFiles.Items.Count == 0)
            {
                MessageBox.Show(
                    this,
                    "Add at least one PDF file before merging.",
                    "Nothing to Merge",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            if (_saveFileDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            string[] inputPaths = _listFiles.Items.Cast<ListViewItem>()
                .Select(item => (string)item.Tag!)
                .ToArray();

            SetBusy(true);

            try
            {
                int pageCount = await _mergeService.MergeAsync(
                    inputPaths,
                    _saveFileDialog.FileName,
                    _chkNewSheet.Checked);

                _statusLabel.Text = $"Merged {inputPaths.Length} files ({pageCount} pages) into {Path.GetFileName(_saveFileDialog.FileName)}.";

                MessageBox.Show(
                    this,
                    $"Successfully merged {inputPaths.Length} files into a {pageCount}-page document.",
                    "Merge Complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _statusLabel.Text = "Merge failed.";

                MessageBox.Show(
                    this,
                    $"The PDFs could not be merged:{Environment.NewLine}{ex.Message}",
                    "Merge Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private void AddFiles(IEnumerable<string> paths)
        {
            foreach (string path in paths)
            {
                if (!path.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                bool alreadyAdded = _listFiles.Items.Cast<ListViewItem>()
                    .Any(item => string.Equals((string)item.Tag!, path, StringComparison.OrdinalIgnoreCase));

                if (!alreadyAdded)
                {
                    ListViewItem listItem = new(Path.GetFileName(path))
                    {
                        Tag = path,
                    };
                    listItem.SubItems.Add(File.GetLastWriteTime(path).ToString("g"));
                    _listFiles.Items.Add(listItem);
                }
            }

            ClearSort();
            UpdateStatus();
            UpdatePreview();
        }

        private void MoveSelectedItem(int offset)
        {
            if (_listFiles.SelectedItems.Count != 1)
            {
                return;
            }

            int index = _listFiles.SelectedIndices[0];
            int newIndex = index + offset;

            if (newIndex < 0 || newIndex >= _listFiles.Items.Count)
            {
                return;
            }

            ListViewItem item = _listFiles.Items[index];
            _listFiles.Items.RemoveAt(index);
            _listFiles.Items.Insert(newIndex, item);
            item.Selected = true;
            item.Focused = true;

            ClearSort();
        }

        private void ListFiles_ColumnClick(object? sender, ColumnClickEventArgs e)
        {
            if (e.Column == _sortColumn)
            {
                _sortOrder = _sortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                _sortColumn = e.Column;
                _sortOrder = SortOrder.Ascending;
            }

            _listFiles.ListViewItemSorter = new PdfFileItemComparer(_sortColumn, _sortOrder);
            _listFiles.Sort();
            UpdateSortIndicators();
        }

        private void ListFiles_Resize(object? sender, EventArgs e)
        {
            const int dateColumnWidth = 140;
            _colDateModified.Width = dateColumnWidth;
            _colName.Width = Math.Max(_listFiles.ClientSize.Width - dateColumnWidth, 150);
        }

        private void ClearSort()
        {
            _sortColumn = -1;
            _sortOrder = SortOrder.None;
            _listFiles.ListViewItemSorter = null;
            UpdateSortIndicators();
        }

        private void UpdateSortIndicators()
        {
            _colName.Text = "Name" + (_sortColumn == 0 ? SortArrow() : string.Empty);
            _colDateModified.Text = "Date Modified" + (_sortColumn == 1 ? SortArrow() : string.Empty);
        }

        private string SortArrow() => _sortOrder == SortOrder.Descending ? " ▼" : " ▲";

        private void SetBusy(bool busy)
        {
            _rootLayout.Enabled = !busy;
            _statusLabel.Text = busy ? "Merging..." : _statusLabel.Text;
            Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
        }

        private void UpdateStatus()
        {
            int count = _listFiles.Items.Count;
            _statusLabel.Text = count == 0
                ? "Ready"
                : $"{count} file(s) ready to merge.";
        }

        private void ChkShowPreview_CheckedChanged(object? sender, EventArgs e)
        {
            _previewSplit.Panel2Collapsed = !_chkShowPreview.Checked;

            if (_chkShowPreview.Checked)
            {
                UpdatePreview();
            }
        }

        private void PreviewPanel_CollapsedChanged(object? sender, EventArgs e)
        {
            int desiredPanel2Width = _previewPanel.IsCollapsed ? PreviewCollapsedWidth : PreviewExpandedWidth;
            _previewSplit.SplitterDistance = Math.Max(0, _previewSplit.Width - desiredPanel2Width - _previewSplit.SplitterWidth);
        }

        private void ListFiles_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            if (!_chkShowPreview.Checked)
            {
                return;
            }

            string? path = _listFiles.SelectedItems.Count > 0
                ? (string)_listFiles.SelectedItems[0].Tag!
                : null;

            if (path is not null)
            {
                _ = _previewPanel.ShowPreviewAsync(path);
            }
            else
            {
                _previewPanel.ClearPreview();
            }
        }

        /// <summary>
        /// Sorts <see cref="ListViewItem"/> rows by name (column 0) or last-modified date
        /// (column 1) in the given direction. Each item's full path is read from its <see cref="ListViewItem.Tag"/>.
        /// </summary>
        private sealed class PdfFileItemComparer : IComparer
        {
            private readonly int _column;
            private readonly SortOrder _order;

            public PdfFileItemComparer(int column, SortOrder order)
            {
                _column = column;
                _order = order;
            }

            public int Compare(object? x, object? y)
            {
                ListViewItem itemX = (ListViewItem)x!;
                ListViewItem itemY = (ListViewItem)y!;

                int result = _column == 1
                    ? DateTime.Compare(GetLastWriteTime(itemX), GetLastWriteTime(itemY))
                    : string.Compare(itemX.Text, itemY.Text, StringComparison.CurrentCultureIgnoreCase);

                return _order == SortOrder.Descending ? -result : result;
            }

            private static DateTime GetLastWriteTime(ListViewItem item) => File.GetLastWriteTime((string)item.Tag!);
        }
    }
}
