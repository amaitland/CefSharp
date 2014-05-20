using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using CefSharp.Example;
using CefSharp.Wpf.Example.Mvvm;

namespace CefSharp.Wpf.Example.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		private const string DefaultUrlForAddedTabs = "https://www.google.com";

		public ObservableCollection<BrowserTabViewModel> BrowserTabs { get; set; }

		public ICommand NewTabCommand { get; private set; }
		public ICommand CloseTabCommand { get; private set; }
		public ICommand ReloadTabCommand { get; private set; }
		public ICommand FocusAddressCommand { get; private set; }

		private BrowserTabViewModel currentTab;
		public BrowserTabViewModel CurrentTab
		{
			get { return currentTab; }
			set { Set(ref currentTab, value, new PropertyChangedEventArgs("CurrentTab")); }
		}

		public MainViewModel()
		{
			BrowserTabs = new ObservableCollection<BrowserTabViewModel>();

			NewTabCommand = new DelegateCommand(NewTab);
			CloseTabCommand = new DelegateCommand<BrowserTabViewModel>(CloseTab);
			ReloadTabCommand = new DelegateCommand<string>(ReloadTab, s => CurrentTab != null);
			FocusAddressCommand = new DelegateCommand(FocusAddress);

			CreateNewTab(ExamplePresenter.DefaultUrl, true);
		}

		private void FocusAddress()
		{
			CurrentTab.IsAddressFocused = true;
		}

		private void ReloadTab(string parameter)
		{
			var forceReload = !string.IsNullOrEmpty(parameter) && string.Compare(parameter, "Force", StringComparison.OrdinalIgnoreCase) == 0;

			CurrentTab.Reload(forceReload);
		}

		public void CloseTab(BrowserTabViewModel viewModel)
		{
			if (BrowserTabs.Count > 0)
			{
				var browserViewModel = viewModel ?? CurrentTab;

				BrowserTabs.Remove(browserViewModel);
				browserViewModel.Dispose();
			}
		}

		private void NewTab()
		{
			CreateNewTab();
		}

		private void CreateNewTab(string url = DefaultUrlForAddedTabs, bool showSideBar = false)
		{
			BrowserTabs.Add(new BrowserTabViewModel(url) { ShowSidebar = showSideBar });

			CurrentTab = BrowserTabs.Last();
		}
	}
}
