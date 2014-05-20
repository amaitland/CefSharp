// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows;
using System.Windows.Input;
using CefSharp.Wpf.Example.ViewModels;
using CefSharp.Wpf.Example.Mvvm;

namespace CefSharp.Wpf.Example
{
	public partial class MainWindow : Window
	{
		private readonly MainViewModel viewModel;

		public ICommand ExitCommand { get; private set; }

		public MainWindow()
		{
			InitializeComponent();

			viewModel = new MainViewModel();

			ViewModelContainer.Content = viewModel;

			ExitCommand = new DelegateCommand(Exit);
		}

		private void Exit()
		{
			Close();
		}
	}
}
