using System.Windows;
using System.Windows.Controls;
using CefSharp;
using CefSharp.Wpf;

namespace WPFHostedInWinForm
{
    /// <summary>
    /// Interaction logic for BrowserUserControl.xaml
    /// </summary>
    public partial class BrowserUserControl : UserControl
    {
        private readonly ChromiumWebBrowser webBrowser;

        public BrowserUserControl()
        {
            InitializeComponent();

            if (null == Application.Current)
            {
                new Application();
            }

            if (!Cef.IsInitialized)
            {
                var browserSettings = new CefSettings
                {
                    PackLoadingDisabled = true,
                    BrowserSubprocessPath = "CefSharp.BrowserSubprocess.exe"
                };

                Cef.Initialize(browserSettings);
            }

            webBrowser = new ChromiumWebBrowser
            {
                Address = "https://www.google.com"
            };
            BrowserGrid.Children.Add(webBrowser);
        }
    }
}
