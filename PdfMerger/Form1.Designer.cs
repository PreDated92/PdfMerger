namespace PdfMerger
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            _tabMain = new TabControl();
            _tabPageMerge = new TabPage();
            _previewSplit = new SplitContainer();
            _rootLayout = new TableLayoutPanel();
            _listFiles = new ListView();
            _colName = new ColumnHeader();
            _colDateModified = new ColumnHeader();
            _buttonLayout = new FlowLayoutPanel();
            _btnAdd = new Button();
            _btnRemove = new Button();
            _btnMoveUp = new Button();
            _btnMoveDown = new Button();
            _btnClear = new Button();
            _chkShowPreview = new CheckBox();
            _previewPanel = new PdfPreviewPanel();
            _bottomLayout = new FlowLayoutPanel();
            _btnMerge = new Button();
            _chkNewSheet = new CheckBox();
            _lblHint = new Label();
            _tabPageDevDocs = new TabPage();
            _tabDevDocs = new TabControl();
            _tabDevDocsListSorting = new TabPage();
            _txtDevDocsListSorting = new TextBox();
            _tabDevDocsPdfPreview = new TabPage();
            _txtDevDocsPdfPreview = new TextBox();
            _statusStrip = new StatusStrip();
            _statusLabel = new ToolStripStatusLabel();
            _openFileDialog = new OpenFileDialog();
            _saveFileDialog = new SaveFileDialog();
            _tabMain.SuspendLayout();
            _tabPageMerge.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_previewSplit).BeginInit();
            _previewSplit.Panel1.SuspendLayout();
            _previewSplit.Panel2.SuspendLayout();
            _previewSplit.SuspendLayout();
            _rootLayout.SuspendLayout();
            _buttonLayout.SuspendLayout();
            _bottomLayout.SuspendLayout();
            _tabPageDevDocs.SuspendLayout();
            _tabDevDocs.SuspendLayout();
            _tabDevDocsListSorting.SuspendLayout();
            _tabDevDocsPdfPreview.SuspendLayout();
            _statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // _rootLayout
            // 
            _rootLayout.ColumnCount = 2;
            _rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _rootLayout.Dock = DockStyle.Fill;
            _rootLayout.Location = new Point(0, 0);
            _rootLayout.Name = "_rootLayout";
            _rootLayout.Padding = new Padding(8);
            _rootLayout.RowCount = 3;
            _rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _rootLayout.Controls.Add(_lblHint, 0, 0);
            _rootLayout.Controls.Add(_listFiles, 0, 1);
            _rootLayout.Controls.Add(_buttonLayout, 1, 1);
            _rootLayout.Controls.Add(_bottomLayout, 0, 2);
            _rootLayout.Size = new Size(800, 428);
            _rootLayout.TabIndex = 0;
            // 
            // _lblHint
            // 
            _lblHint.AutoSize = true;
            _lblHint.Anchor = AnchorStyles.Left;
            _lblHint.Margin = new Padding(3, 3, 3, 6);
            _lblHint.Name = "_lblHint";
            _lblHint.Text = "Drag and drop PDF files here, or use Add Files. Reorder to set merge order.";
            // 
            // _listFiles
            // 
            _listFiles.AllowDrop = true;
            _listFiles.Columns.AddRange(new ColumnHeader[] { _colName, _colDateModified });
            _listFiles.Dock = DockStyle.Fill;
            _listFiles.FullRowSelect = true;
            _listFiles.HeaderStyle = ColumnHeaderStyle.Clickable;
            _listFiles.HideSelection = false;
            _listFiles.MultiSelect = true;
            _listFiles.Margin = new Padding(3);
            _listFiles.Name = "_listFiles";
            _listFiles.UseCompatibleStateImageBehavior = false;
            _listFiles.View = View.Details;
            _listFiles.TabIndex = 1;
            _listFiles.AccessibleName = "PDF file list";
            _listFiles.AccessibleDescription = "List of PDF files to merge in order";
            _listFiles.DragEnter += ListFiles_DragEnter;
            _listFiles.DragDrop += ListFiles_DragDrop;
            _listFiles.ColumnClick += ListFiles_ColumnClick;
            _listFiles.Resize += ListFiles_Resize;
            _listFiles.SelectedIndexChanged += ListFiles_SelectedIndexChanged;
            // 
            // _colName
            // 
            _colName.Text = "Name";
            _colName.Width = 400;
            // 
            // _colDateModified
            // 
            _colDateModified.Text = "Date Modified";
            _colDateModified.Width = 140;
            // 
            // _buttonLayout
            // 
            _buttonLayout.AutoSize = true;
            _buttonLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _buttonLayout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _buttonLayout.FlowDirection = FlowDirection.TopDown;
            _buttonLayout.Margin = new Padding(3);
            _buttonLayout.Name = "_buttonLayout";
            _buttonLayout.Controls.Add(_btnAdd);
            _buttonLayout.Controls.Add(_btnRemove);
            _buttonLayout.Controls.Add(_btnMoveUp);
            _buttonLayout.Controls.Add(_btnMoveDown);
            _buttonLayout.Controls.Add(_btnClear);
            _buttonLayout.Controls.Add(_chkShowPreview);
            _buttonLayout.TabIndex = 2;
            // 
            // _btnAdd
            // 
            _btnAdd.AutoSize = true;
            _btnAdd.Margin = new Padding(3, 3, 3, 8);
            _btnAdd.MinimumSize = new Size(110, 30);
            _btnAdd.Name = "_btnAdd";
            _btnAdd.Text = "&Add Files...";
            _btnAdd.UseVisualStyleBackColor = true;
            _btnAdd.AccessibleName = "Add files";
            _btnAdd.Click += BtnAdd_Click;
            // 
            // _btnRemove
            // 
            _btnRemove.AutoSize = true;
            _btnRemove.Margin = new Padding(3);
            _btnRemove.MinimumSize = new Size(110, 30);
            _btnRemove.Name = "_btnRemove";
            _btnRemove.Text = "&Remove";
            _btnRemove.UseVisualStyleBackColor = true;
            _btnRemove.AccessibleName = "Remove selected files";
            _btnRemove.Click += BtnRemove_Click;
            // 
            // _btnMoveUp
            // 
            _btnMoveUp.AutoSize = true;
            _btnMoveUp.Margin = new Padding(3);
            _btnMoveUp.MinimumSize = new Size(110, 30);
            _btnMoveUp.Name = "_btnMoveUp";
            _btnMoveUp.Text = "Move &Up";
            _btnMoveUp.UseVisualStyleBackColor = true;
            _btnMoveUp.AccessibleName = "Move selected file up";
            _btnMoveUp.Click += BtnMoveUp_Click;
            // 
            // _btnMoveDown
            // 
            _btnMoveDown.AutoSize = true;
            _btnMoveDown.Margin = new Padding(3, 3, 3, 8);
            _btnMoveDown.MinimumSize = new Size(110, 30);
            _btnMoveDown.Name = "_btnMoveDown";
            _btnMoveDown.Text = "Move &Down";
            _btnMoveDown.UseVisualStyleBackColor = true;
            _btnMoveDown.AccessibleName = "Move selected file down";
            _btnMoveDown.Click += BtnMoveDown_Click;
            // 
            // _btnClear
            // 
            _btnClear.AutoSize = true;
            _btnClear.Margin = new Padding(3);
            _btnClear.MinimumSize = new Size(110, 30);
            _btnClear.Name = "_btnClear";
            _btnClear.Text = "&Clear All";
            _btnClear.UseVisualStyleBackColor = true;
            _btnClear.AccessibleName = "Clear all files";
            _btnClear.Click += BtnClear_Click;
            // 
            // _chkShowPreview
            // 
            _chkShowPreview.AutoSize = true;
            _chkShowPreview.Margin = new Padding(3, 14, 3, 3);
            _chkShowPreview.Name = "_chkShowPreview";
            _chkShowPreview.Text = "Show &Preview";
            _chkShowPreview.UseVisualStyleBackColor = true;
            _chkShowPreview.AccessibleName = "Show PDF preview panel";
            _chkShowPreview.AccessibleDescription = "Shows a collapsible panel that previews the selected PDF file";
            _chkShowPreview.CheckedChanged += ChkShowPreview_CheckedChanged;
            // 
            // _bottomLayout
            // 
            _bottomLayout.AutoSize = true;
            _bottomLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _bottomLayout.Anchor = AnchorStyles.Right;
            _rootLayout.SetColumnSpan(_bottomLayout, 2);
            _bottomLayout.FlowDirection = FlowDirection.RightToLeft;
            _bottomLayout.Margin = new Padding(3, 6, 3, 3);
            _bottomLayout.Name = "_bottomLayout";
            _bottomLayout.Controls.Add(_btnMerge);
            _bottomLayout.Controls.Add(_chkNewSheet);
            _bottomLayout.TabIndex = 3;
            // 
            // _btnMerge
            // 
            _btnMerge.AutoSize = true;
            _btnMerge.Margin = new Padding(3);
            _btnMerge.MinimumSize = new Size(140, 34);
            _btnMerge.Name = "_btnMerge";
            _btnMerge.Text = "&Merge PDFs...";
            _btnMerge.UseVisualStyleBackColor = true;
            _btnMerge.AccessibleName = "Merge PDFs";
            _btnMerge.Click += BtnMerge_Click;
            // 
            // _chkNewSheet
            // 
            _chkNewSheet.AutoSize = true;
            _chkNewSheet.Anchor = AnchorStyles.None;
            _chkNewSheet.Margin = new Padding(3, 9, 12, 3);
            _chkNewSheet.Name = "_chkNewSheet";
            _chkNewSheet.Text = "Start each document on a &new sheet (duplex)";
            _chkNewSheet.UseVisualStyleBackColor = true;
            _chkNewSheet.AccessibleName = "Start each document on a new sheet";
            _chkNewSheet.AccessibleDescription = "Inserts a blank page after a document with an odd page count so the next document starts on a new sheet when printing double-sided";
            // 
            // _tabMain
            // 
            _tabMain.Controls.Add(_tabPageMerge);
            _tabMain.Controls.Add(_tabPageDevDocs);
            _tabMain.Dock = DockStyle.Fill;
            _tabMain.Location = new Point(0, 0);
            _tabMain.Name = "_tabMain";
            _tabMain.SelectedIndex = 0;
            _tabMain.Size = new Size(800, 428);
            _tabMain.TabIndex = 0;
            // 
            // _tabPageMerge
            // 
            _tabPageMerge.Controls.Add(_previewSplit);
            _tabPageMerge.Location = new Point(4, 24);
            _tabPageMerge.Name = "_tabPageMerge";
            _tabPageMerge.Size = new Size(792, 400);
            _tabPageMerge.TabIndex = 0;
            _tabPageMerge.Text = "Merge PDFs";
            _tabPageMerge.UseVisualStyleBackColor = true;
            // 
            // _previewSplit
            // 
            _previewSplit.Dock = DockStyle.Fill;
            _previewSplit.FixedPanel = FixedPanel.Panel2;
            _previewSplit.Location = new Point(0, 0);
            _previewSplit.Name = "_previewSplit";
            // 
            // _previewSplit.Panel1
            // 
            _previewSplit.Panel1.Controls.Add(_rootLayout);
            // 
            // _previewSplit.Panel2
            // 
            _previewSplit.Panel2.Controls.Add(_previewPanel);
            _previewSplit.Panel2Collapsed = true;
            _previewSplit.Size = new Size(792, 400);
            _previewSplit.SplitterDistance = 512;
            _previewSplit.TabIndex = 0;
            // 
            // _previewPanel
            // 
            _previewPanel.Dock = DockStyle.Fill;
            _previewPanel.Location = new Point(0, 0);
            _previewPanel.Name = "_previewPanel";
            _previewPanel.Size = new Size(280, 400);
            _previewPanel.TabIndex = 0;
            _previewPanel.CollapsedChanged += PreviewPanel_CollapsedChanged;
            // 
            // _tabPageDevDocs
            // 
            _tabPageDevDocs.Controls.Add(_tabDevDocs);
            _tabPageDevDocs.Location = new Point(4, 24);
            _tabPageDevDocs.Name = "_tabPageDevDocs";
            _tabPageDevDocs.Padding = new Padding(8);
            _tabPageDevDocs.Size = new Size(792, 400);
            _tabPageDevDocs.TabIndex = 1;
            _tabPageDevDocs.Text = "Developer Documentation";
            _tabPageDevDocs.UseVisualStyleBackColor = true;
            // 
            // _tabDevDocs
            // 
            _tabDevDocs.Controls.Add(_tabDevDocsListSorting);
            _tabDevDocs.Controls.Add(_tabDevDocsPdfPreview);
            _tabDevDocs.Dock = DockStyle.Fill;
            _tabDevDocs.Location = new Point(8, 8);
            _tabDevDocs.Name = "_tabDevDocs";
            _tabDevDocs.SelectedIndex = 0;
            _tabDevDocs.Size = new Size(776, 384);
            _tabDevDocs.TabIndex = 0;
            // 
            // _tabDevDocsListSorting
            // 
            _tabDevDocsListSorting.Controls.Add(_txtDevDocsListSorting);
            _tabDevDocsListSorting.Location = new Point(4, 24);
            _tabDevDocsListSorting.Name = "_tabDevDocsListSorting";
            _tabDevDocsListSorting.Size = new Size(768, 356);
            _tabDevDocsListSorting.TabIndex = 0;
            _tabDevDocsListSorting.Text = "List Sorting";
            _tabDevDocsListSorting.UseVisualStyleBackColor = true;
            // 
            // _txtDevDocsListSorting
            // 
            _txtDevDocsListSorting.BackColor = SystemColors.Window;
            _txtDevDocsListSorting.Dock = DockStyle.Fill;
            _txtDevDocsListSorting.Location = new Point(0, 0);
            _txtDevDocsListSorting.Multiline = true;
            _txtDevDocsListSorting.Name = "_txtDevDocsListSorting";
            _txtDevDocsListSorting.ReadOnly = true;
            _txtDevDocsListSorting.ScrollBars = ScrollBars.Vertical;
            _txtDevDocsListSorting.Size = new Size(768, 356);
            _txtDevDocsListSorting.TabIndex = 0;
            _txtDevDocsListSorting.AccessibleName = "List sorting developer documentation notes";
            // 
            // _tabDevDocsPdfPreview
            // 
            _tabDevDocsPdfPreview.Controls.Add(_txtDevDocsPdfPreview);
            _tabDevDocsPdfPreview.Location = new Point(4, 24);
            _tabDevDocsPdfPreview.Name = "_tabDevDocsPdfPreview";
            _tabDevDocsPdfPreview.Size = new Size(768, 356);
            _tabDevDocsPdfPreview.TabIndex = 1;
            _tabDevDocsPdfPreview.Text = "PDF Preview";
            _tabDevDocsPdfPreview.UseVisualStyleBackColor = true;
            // 
            // _txtDevDocsPdfPreview
            // 
            _txtDevDocsPdfPreview.BackColor = SystemColors.Window;
            _txtDevDocsPdfPreview.Dock = DockStyle.Fill;
            _txtDevDocsPdfPreview.Location = new Point(0, 0);
            _txtDevDocsPdfPreview.Multiline = true;
            _txtDevDocsPdfPreview.Name = "_txtDevDocsPdfPreview";
            _txtDevDocsPdfPreview.ReadOnly = true;
            _txtDevDocsPdfPreview.ScrollBars = ScrollBars.Vertical;
            _txtDevDocsPdfPreview.Size = new Size(768, 356);
            _txtDevDocsPdfPreview.TabIndex = 0;
            _txtDevDocsPdfPreview.AccessibleName = "PDF preview developer documentation notes";
            // 
            // _statusStrip
            // 
            _statusStrip.Items.AddRange(new ToolStripItem[] { _statusLabel });
            _statusStrip.Location = new Point(0, 428);
            _statusStrip.Name = "_statusStrip";
            _statusStrip.Size = new Size(800, 22);
            _statusStrip.TabIndex = 1;
            // 
            // _statusLabel
            // 
            _statusLabel.Name = "_statusLabel";
            _statusLabel.Text = "Ready";
            // 
            // _openFileDialog
            // 
            _openFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            _openFileDialog.Multiselect = true;
            _openFileDialog.Title = "Add PDF Files";
            // 
            // _saveFileDialog
            // 
            _saveFileDialog.DefaultExt = "pdf";
            _saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            _saveFileDialog.Title = "Save Merged PDF";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(_tabMain);
            Controls.Add(_statusStrip);
            MinimumSize = new Size(520, 360);
            Name = "Form1";
            Text = "PDF Merger";
            _tabMain.ResumeLayout(false);
            _tabPageMerge.ResumeLayout(false);
            _previewSplit.Panel1.ResumeLayout(false);
            _previewSplit.Panel2.ResumeLayout(false);
            _previewSplit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_previewSplit).EndInit();
            _rootLayout.ResumeLayout(false);
            _rootLayout.PerformLayout();
            _buttonLayout.ResumeLayout(false);
            _buttonLayout.PerformLayout();
            _bottomLayout.ResumeLayout(false);
            _bottomLayout.PerformLayout();
            _tabPageDevDocs.ResumeLayout(false);
            _tabDevDocs.ResumeLayout(false);
            _tabDevDocsListSorting.ResumeLayout(false);
            _tabDevDocsListSorting.PerformLayout();
            _tabDevDocsPdfPreview.ResumeLayout(false);
            _tabDevDocsPdfPreview.PerformLayout();
            _statusStrip.ResumeLayout(false);
            _statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl _tabMain;
        private TabPage _tabPageMerge;
        private TabPage _tabPageDevDocs;
        private TabControl _tabDevDocs;
        private TabPage _tabDevDocsListSorting;
        private TextBox _txtDevDocsListSorting;
        private TabPage _tabDevDocsPdfPreview;
        private TextBox _txtDevDocsPdfPreview;
        private SplitContainer _previewSplit;
        private PdfPreviewPanel _previewPanel;
        private CheckBox _chkShowPreview;
        private TableLayoutPanel _rootLayout;
        private ListView _listFiles;
        private ColumnHeader _colName;
        private ColumnHeader _colDateModified;
        private FlowLayoutPanel _buttonLayout;
        private Button _btnAdd;
        private Button _btnRemove;
        private Button _btnMoveUp;
        private Button _btnMoveDown;
        private Button _btnClear;
        private FlowLayoutPanel _bottomLayout;
        private Button _btnMerge;
        private CheckBox _chkNewSheet;
        private Label _lblHint;
        private StatusStrip _statusStrip;
        private ToolStripStatusLabel _statusLabel;
        private OpenFileDialog _openFileDialog;
        private SaveFileDialog _saveFileDialog;
    }
}
