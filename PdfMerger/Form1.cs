using System.Collections;
using PdfMerger.Core;

namespace PdfMerger
{
    public partial class Form1 : Form
    {
        private readonly PdfMergeService _mergeService = new();
        private int _sortColumn = -1;
        private SortOrder _sortOrder = SortOrder.None;

        public Form1()
        {
            InitializeComponent();
            ListFiles_Resize(_listFiles, EventArgs.Empty);
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
