using System.Windows.Forms;

namespace WPFHostedInWinForm
{
    public partial class WinFormsExampleHost : Form
    {
        public WinFormsExampleHost()
        {
            InitializeComponent();

            var wpfWindow = new WpfWindow();

            //ElementHost.EnableModelessKeyboardInterop(wpfwindow);

            wpfWindow.ShowDialog();

            Load += Form1Load;
        }

        private void Form1Load(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}
