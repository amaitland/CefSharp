using System.Windows;

namespace WPFHostedInWinForm
{
    /// <summary>
    /// Interaction logic for WpfWindow.xaml
    /// </summary>
    public partial class WpfWindow : Window
    {
        public WpfWindow()
        {
            InitializeComponent();

            var browserUserControl = new BrowserUserControl();
            MainGrid.Children.Add(browserUserControl);

        }
    }
}
