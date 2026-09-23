using Microsoft.Web.WebView2.Core;

namespace PdfMerger
{
    /// <summary>
    /// Self-contained PDF preview panel backed by an embedded WebView2 control. This
    /// control has no knowledge of <see cref="Form1"/>, the file list, or the container
    /// hosting it — callers give it a file path via <see cref="ShowPreviewAsync"/>, and
    /// the collapse toggle is surfaced through <see cref="IsCollapsed"/> and
    /// <see cref="CollapsedChanged"/> so a host can react (e.g. by resizing a
    /// SplitContainer) without this control reaching into its parent.
    /// </summary>
    public partial class PdfPreviewPanel : UserControl
    {
        private bool _webViewInitialized;

        public PdfPreviewPanel()
        {
            InitializeComponent();
        }

        public bool IsCollapsed { get; private set; }

        public event EventHandler? CollapsedChanged;

        public void SetCollapsed(bool collapsed)
        {
            if (IsCollapsed == collapsed)
            {
                return;
            }

            IsCollapsed = collapsed;
            ApplyCollapsedState();
            CollapsedChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task ShowPreviewAsync(string pdfPath)
        {
            if (!await EnsureWebViewReadyAsync())
            {
                return;
            }

            _lblPlaceholder.Visible = false;
            _webView.Visible = !IsCollapsed;
            _webView.CoreWebView2.Navigate(new Uri(pdfPath).AbsoluteUri);
        }

        public void ClearPreview()
        {
            _webView.Visible = false;
            _lblPlaceholder.Text = "No file selected.";
            _lblPlaceholder.Visible = true;

            if (_webViewInitialized)
            {
                _webView.CoreWebView2.Navigate("about:blank");
            }
        }

        private void BtnToggle_Click(object? sender, EventArgs e)
        {
            SetCollapsed(!IsCollapsed);
        }

        private void ApplyCollapsedState()
        {
            _btnToggle.Text = IsCollapsed ? "▶" : "◀";
            _lblHeader.Visible = !IsCollapsed;
            _webView.Visible = !IsCollapsed && !_lblPlaceholder.Visible;
        }

        private async Task<bool> EnsureWebViewReadyAsync()
        {
            if (_webViewInitialized)
            {
                return true;
            }

            try
            {
                await _webView.EnsureCoreWebView2Async();
                _webViewInitialized = true;
                return true;
            }
            catch (Exception ex)
            {
                _lblPlaceholder.Text = $"PDF preview is unavailable: {ex.Message}";
                _lblPlaceholder.Visible = true;
                return false;
            }
        }
    }
}
