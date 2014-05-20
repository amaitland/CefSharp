﻿// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Example;
using CefSharp.Wpf.Example.Mvvm;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CefSharp.Wpf.Example.ViewModels
{
    public class BrowserTabViewModel : ViewModelBase
    {
        private string address;
        public string Address
        {
            get { return address; }
            set { Set(ref address, value, new PropertyChangedEventArgs("Address")); }
        }

        private string addressEditable;
        public string AddressEditable
        {
            get { return addressEditable; }
            set { Set(ref addressEditable, value, new PropertyChangedEventArgs("AddressEditable")); }
        }

        private string outputMessage;
        public string OutputMessage
        {
            get { return outputMessage; }
            set { Set(ref outputMessage, value, new PropertyChangedEventArgs("OutputMessage")); }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { Set(ref title, value, new PropertyChangedEventArgs("Title")); }
        }

        private IWpfWebBrowser webBrowser;
        public IWpfWebBrowser WebBrowser
        {
            get { return webBrowser; }
            set { Set(ref webBrowser, value, new PropertyChangedEventArgs("WebBrowser")); }
        }

        private object evaluateJavaScriptResult;

        public object EvaluateJavaScriptResult
        {
            get { return evaluateJavaScriptResult; }
            set { Set(ref evaluateJavaScriptResult, value, new PropertyChangedEventArgs("EvaluateJavaScriptResult")); }
        }

        private bool showSidebar;
        public bool ShowSidebar
        {
            get { return showSidebar; }
            set { Set(ref showSidebar, value, new PropertyChangedEventArgs("ShowSidebar")); }
        }

        private bool isAddressFocused;
        public bool IsAddressFocused
        {
            get { return isAddressFocused; }
            set { Set(ref isAddressFocused, value, new PropertyChangedEventArgs("IsAddressFocused")); }
        }

        public ICommand GoCommand { get; set; }
        public ICommand HomeCommand { get; set; }
        public ICommand ExecuteJavaScriptCommand { get; set; }
        public ICommand EvaluateJavaScriptCommand { get; set; }

        public BrowserTabViewModel(string address)
        {
            Address = address;

            GoCommand = new DelegateCommand(Go, () => !String.IsNullOrWhiteSpace(Address));
            HomeCommand = new DelegateCommand(() => Address = ExamplePresenter.DefaultUrl);
            ExecuteJavaScriptCommand = new DelegateCommand<string>(ExecuteJavaScript, s => !String.IsNullOrWhiteSpace(s));
            EvaluateJavaScriptCommand = new DelegateCommand<string>(EvaluateJavaScript, s => !String.IsNullOrWhiteSpace(s));

            PropertyChanged += OnPropertyChanged;

            var version = String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);
            OutputMessage = version;
        }

        private void EvaluateJavaScript(string s)
        {
            try
            {
                EvaluateJavaScriptResult = webBrowser.EvaluateScript(s) ?? "null";
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while evaluating Javascript: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteJavaScript(string s)
        {
            try
            {
                webBrowser.ExecuteScriptAsync(s);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while executing Javascript: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Address":
                    AddressEditable = Address;
                    break;

                case "WebBrowser":
                    if (WebBrowser != null)
                    {
                        WebBrowser.ConsoleMessage += OnWebBrowserConsoleMessage;
                        WebBrowser.LoadError += OnWebBrowserLoadError;

                        // TODO: This is a bit of a hack. It would be nicer/cleaner to give the webBrowser focus in the Go()
                        // TODO: method, but it seems like "something" gets messed up (= doesn't work correctly) if we give it
                        // TODO: focus "too early" in the loading process...
                        WebBrowser.FrameLoadEnd += OnWebBrowserFrameLoadEnd;
                    }

                    break;
            }
        }

        private void OnWebBrowserConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            OutputMessage = e.Message;
        }

        private void OnWebBrowserLoadError(string failedUrl, CefErrorCode errorCode, string errorText)
        {
            // Don't display an error for downloaded files where the user aborted the download.
            if (errorCode == CefErrorCode.Aborted)
                return;

            var errorMessage = "<html><body><h2>Failed to load URL " + failedUrl +
                  " with error " + errorText + " (" + errorCode +
                  ").</h2></body></html>";

            webBrowser.LoadHtml(errorMessage, failedUrl);
        }

        private void OnWebBrowserFrameLoadEnd(object sender, FrameLoadEndEventArgs url)
        {
            var browser = webBrowser;
            if (browser != null)
            {
                Application.Current.Dispatcher.BeginInvoke((Action)(() => browser.Focus()));
            }
        }

        private void Go()
        {
            Address = AddressEditable;

            // Part of the Focus hack further described in the OnPropertyChanged() method...
            Keyboard.ClearFocus();
        }

        public void Reload(bool ignoreCache)
        {
            webBrowser.Reload(ignoreCache);
        }

        protected override void DoDispose(bool isDisposing)
        {
            if (webBrowser != null)
            {
                WebBrowser.ConsoleMessage -= OnWebBrowserConsoleMessage;
                WebBrowser.LoadError -= OnWebBrowserLoadError;
                WebBrowser.FrameLoadEnd -= OnWebBrowserFrameLoadEnd;
            }
            DisposeMember(ref webBrowser);

            base.DoDispose(isDisposing);
        }
    }
}
