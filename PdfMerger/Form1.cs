using PdfMerger.Core;

namespace PdfMerger
{
    public partial class Form1 : Form
    {
        private readonly PdfMergeService _mergeService = new();

        public Form1()
        {
            InitializeComponent();
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

            string[] inputPaths = _listFiles.Items.Cast<PdfFileItem>()
                .Select(item => item.FullPath)
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

                bool alreadyAdded = _listFiles.Items.Cast<PdfFileItem>()
                    .Any(item => string.Equals(item.FullPath, path, StringComparison.OrdinalIgnoreCase));

                if (!alreadyAdded)
                {
                    _listFiles.Items.Add(new PdfFileItem(path));
                }
            }

            UpdateStatus();
        }

        private void MoveSelectedItem(int offset)
        {
            if (_listFiles.SelectedItems.Count != 1)
            {
                return;
            }

            int index = _listFiles.SelectedIndex;
            int newIndex = index + offset;

            if (newIndex < 0 || newIndex >= _listFiles.Items.Count)
            {
                return;
            }

            object item = _listFiles.Items[index];
            _listFiles.Items.RemoveAt(index);
            _listFiles.Items.Insert(newIndex, item);
            _listFiles.SelectedIndex = newIndex;
        }

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
        /// Represents a PDF file entry in the list, displaying the file name while retaining the full path.
        /// </summary>
        private sealed class PdfFileItem
        {
            public PdfFileItem(string fullPath)
            {
                FullPath = fullPath;
            }

            public string FullPath { get; }

            public override string ToString() => Path.GetFileName(FullPath);
        }
    }
}
