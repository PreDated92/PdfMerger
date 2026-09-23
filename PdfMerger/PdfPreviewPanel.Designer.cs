namespace PdfMerger
{
    partial class PdfPreviewPanel
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

        #region Component Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _header = new Panel();
            _lblHeader = new Label();
            _btnToggle = new Button();
            _lblPlaceholder = new Label();
            _webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            _header.SuspendLayout();
            SuspendLayout();
            //
            // _header
            //
            _header.Controls.Add(_lblHeader);
            _header.Controls.Add(_btnToggle);
            _header.Dock = DockStyle.Top;
            _header.Height = 26;
            _header.Name = "_header";
            _header.TabIndex = 0;
            //
            // _lblHeader
            //
            _lblHeader.Anchor = AnchorStyles.Left;
            _lblHeader.AutoSize = true;
            _lblHeader.Location = new Point(4, 6);
            _lblHeader.Name = "_lblHeader";
            _lblHeader.Text = "Preview";
            //
            // _btnToggle
            //
            _btnToggle.Anchor = AnchorStyles.Right;
            _btnToggle.FlatStyle = FlatStyle.Flat;
            _btnToggle.Location = new Point(252, 1);
            _btnToggle.Name = "_btnToggle";
            _btnToggle.Size = new Size(24, 24);
            _btnToggle.TabIndex = 1;
            _btnToggle.Text = "◀";
            _btnToggle.UseVisualStyleBackColor = true;
            _btnToggle.AccessibleName = "Collapse or expand the preview panel";
            _btnToggle.Click += BtnToggle_Click;
            //
            // _lblPlaceholder
            //
            _lblPlaceholder.Dock = DockStyle.Fill;
            _lblPlaceholder.Location = new Point(0, 26);
            _lblPlaceholder.Name = "_lblPlaceholder";
            _lblPlaceholder.Text = "No file selected.";
            _lblPlaceholder.TextAlign = ContentAlignment.MiddleCenter;
            _lblPlaceholder.AccessibleName = "Preview placeholder";
            //
            // _webView
            //
            _webView.Dock = DockStyle.Fill;
            _webView.Location = new Point(0, 26);
            _webView.Name = "_webView";
            _webView.Size = new Size(280, 374);
            _webView.TabIndex = 2;
            _webView.Visible = false;
            //
            // PdfPreviewPanel
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_webView);
            Controls.Add(_lblPlaceholder);
            Controls.Add(_header);
            Name = "PdfPreviewPanel";
            Size = new Size(280, 400);
            _header.ResumeLayout(false);
            _header.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel _header;
        private Label _lblHeader;
        private Button _btnToggle;
        private Label _lblPlaceholder;
        private Microsoft.Web.WebView2.WinForms.WebView2 _webView;
    }
}
