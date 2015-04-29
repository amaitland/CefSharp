using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace CefSharp.Wpf.Example
{
	public partial class LeakyWindow : Window
	{
		private BackgroundWorker worker;

		public LeakyWindow()
		{
			InitializeComponent();

			worker = new BackgroundWorker();
			worker.DoWork += WorkerDoWorkAsync;
			worker.RunWorkerAsync();
		}

		private void WorkerDoWorkAsync(object sender, DoWorkEventArgs e)
		{       
			var flag = false;

			while (true)
			{
				Dispatcher.Invoke(() =>
				{
					if (PlayerCanvas.Children.Count > 0)
					{
						var browserToDispose = PlayerCanvas.Children[0];
						PlayerCanvas.Children.RemoveAt(0);
						((IDisposable)browserToDispose).Dispose();
					}

					var browserToStage = CreateBrowser(flag ? "http://www.google.com" : "https://github.com/cefsharp/CefSharp/issues/984");
					PlayerCanvas.Children.Insert(0, browserToStage);
				});

				Thread.Sleep(7000);
				flag = !flag;
			}
		}

		private ChromiumWebBrowser CreateBrowser(string url)
		{
			var browser = new ChromiumWebBrowser();
			browser.Width = 1024;
			browser.Height = 768;
			browser.Address = url;
			return browser;
		}
	}
}
