// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CefSharp.Example;
using CefSharp.Wpf.Example.Mvvm;

namespace CefSharp.Wpf.Example
{
	/// <summary>
	/// A basic Authentication Dialog that can be used for WebRequest's that require a UserName/Password
	/// </summary>
	public partial class AuthDialog : Window, INotifyPropertyChanged, IAuthDialog
	{
		private static readonly PropertyChangedEventArgs UserNameChangedEventArgs = ViewModelBase.GetPropertyChangedEventArgs<AuthDialog>(x => x.UserName);
		private static readonly PropertyChangedEventArgs PasswordChangedEventArgs = ViewModelBase.GetPropertyChangedEventArgs<AuthDialog>(x => x.Password);

		public event PropertyChangedEventHandler PropertyChanged;

		private string userName;
		
		public string UserName
		{
			get { return userName; }
			set { ChangeAndNotify(ref userName, value, UserNameChangedEventArgs); }
		}

		private string password;
		public string Password
		{
			get { return password; }
			set { ChangeAndNotify(ref password, value, PasswordChangedEventArgs); }
		}

		public AuthDialog()
		{
			InitializeComponent();

			CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, CloseDialog));

			DataContext = this;
		}

		private void CloseDialog(object sender, ExecutedRoutedEventArgs e)
		{
			if (e.Parameter != null)
			{
				DialogResult = bool.Parse(e.Parameter.ToString());
			}
			Close();
		}

		private void PasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
		{
			var passwordBox = (PasswordBox)sender;
			Password = passwordBox.Password;
		}

		public bool ChangeAndNotify<T>(ref T field, T value, PropertyChangedEventArgs args)
		{
			if (EqualityComparer<T>.Default.Equals(field, value))
			{
				return false;
			}

			field = value;

			var handler = PropertyChanged;

			if (handler != null)
			{
				handler(this, args);
			}

			return true;
		}
	}
}
