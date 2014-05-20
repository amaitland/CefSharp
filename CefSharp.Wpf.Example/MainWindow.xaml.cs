// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CefSharp.Example;
using CefSharp.Wpf.Example.ViewModels;
using CefSharp.Wpf.Example.Mvvm;

namespace CefSharp.Wpf.Example
{
    public partial class MainWindow : Window
    {
        private const string DefaultUrlForAddedTabs = "https://www.google.com";

        public ObservableCollection<BrowserTabViewModel> BrowserTabs { get; set; }

        public ICommand NewTabCommand { get; private set; }
        public ICommand CloseTabCommand { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            BrowserTabs = new ObservableCollection<BrowserTabViewModel>();

            NewTabCommand = new DelegateCommand(OpenNewTab);
            CloseTabCommand = new DelegateCommand<BrowserTabViewModel>(CloseTab);

            Loaded += MainWindowLoaded;
        }

        private void CloseTab(BrowserTabViewModel viewModel)
        {
            if (BrowserTabs.Count > 0)
            {
                var browserViewModel = viewModel ?? (BrowserTabViewModel)TabControl.SelectedContent;

                BrowserTabs.Remove(browserViewModel);

                browserViewModel.WebBrowser.Dispose();
            }
        }

        private void OpenNewTab()
        {
            CreateNewTab();

            TabControl.SelectedIndex = TabControl.Items.Count - 1;
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            CreateNewTab(ExamplePresenter.DefaultUrl, true);
        }

        private void CreateNewTab(string url = DefaultUrlForAddedTabs, bool showSideBar = false)
        {
            BrowserTabs.Add(new BrowserTabViewModel(url) { ShowSidebar = showSideBar });
        }
    }
}
