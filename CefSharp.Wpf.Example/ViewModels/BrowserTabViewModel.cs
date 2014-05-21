// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
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
        private static readonly PropertyChangedEventArgs AddressChangedEventArgs = GetArgs<BrowserTabViewModel>(x => x.Address);
        private static readonly PropertyChangedEventArgs AddressEditableChangedEventArgs = GetArgs<BrowserTabViewModel>(x => x.AddressEditable);
        private static readonly PropertyChangedEventArgs OutputMessageChangedEventArgs = GetArgs<BrowserTabViewModel>(x => x.OutputMessage);
        private static readonly PropertyChangedEventArgs TitleChangedEventArgs = GetArgs<BrowserTabViewModel>(x => x.Title);
        private static readonly PropertyChangedEventArgs WebBrowserChangedEventArgs = GetArgs<BrowserTabViewModel>(x => x.WebBrowser);
        private static readonly PropertyChangedEventArgs EvaluateJavaScriptResultChangedEventArgs = GetArgs<BrowserTabViewModel>(x => x.EvaluateJavaScriptResult);
        private static readonly PropertyChangedEventArgs ShowSidebarChangedEventArgs = GetArgs<BrowserTabViewModel>(x => x.ShowSidebar);
        private static readonly PropertyChangedEventArgs IsAddressFocusedEventArgs = GetArgs<BrowserTabViewModel>(x => x.IsAddressFocused);
        
        private string address;
        public string Address
        {
            get { return address; }
            set { Set(ref address, value, AddressChangedEventArgs); }
        }

        private string addressEditable;
        public string AddressEditable
        {
            get { return addressEditable; }
            set { Set(ref addressEditable, value, AddressEditableChangedEventArgs); }
        }

        private string outputMessage;
        public string OutputMessage
        {
            get { return outputMessage; }
            set { Set(ref outputMessage, value, OutputMessageChangedEventArgs); }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set {Set( ref title, value, TitleChangedEventArgs); }
        }

        private IWpfWebBrowser webBrowser;
        public IWpfWebBrowser WebBrowser
        {
            get { return webBrowser; }
            set {Set( ref webBrowser, value, WebBrowserChangedEventArgs); }
        }

        private object evaluateJavaScriptResult;
        public object EvaluateJavaScriptResult
        {
            get { return evaluateJavaScriptResult; }
            set { Set(ref evaluateJavaScriptResult, value, EvaluateJavaScriptResultChangedEventArgs); }
        }

        private bool showSidebar;
        public bool ShowSidebar
        {
            get { return showSidebar; }
            set { Set(ref showSidebar, value, ShowSidebarChangedEventArgs); }
        }

        private bool isAddressFocused;
        public bool IsAddressFocused
        {
            get { return isAddressFocused; }
            set { Set(ref isAddressFocused, value, IsAddressFocusedEventArgs); }
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

        private void OnWebBrowserLoadError(object sender, LoadErrorEventArgs args)
        {
            // Don't display an error for downloaded files where the user aborted the download.
            if (args.ErrorCode == CefErrorCode.Aborted)
                return;

            var errorMessage = "<html><body><h2>Failed to load URL " + args.FailedUrl +
                  " with error " + args.ErrorText + " (" + args.ErrorCode +
                  ").</h2></body></html>";

            webBrowser.LoadHtml(errorMessage, args.FailedUrl);
        }

        private void OnWebBrowserFrameLoadEnd(object sender, FrameLoadEndEventArgs args)
        {
            var browser = webBrowser;
            if (browser != null)
            {
                DoInUi(() => browser.Focus());
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
