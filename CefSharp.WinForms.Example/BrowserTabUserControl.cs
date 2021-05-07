// Copyright © 2010 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp.Example;
using CefSharp.Example.Handlers;
using CefSharp.Example.JavascriptBinding;
using CefSharp.WinForms.Example.Handlers;
using CefSharp.WinForms.Experimental;

namespace CefSharp.WinForms.Example
{
    public partial class BrowserTabUserControl : UserControl
    {
        public IWinFormsWebBrowser Browser { get; private set; }
        private bool multiThreadedMessageLoopEnabled;

        public BrowserTabUserControl(Action<string, int?> openNewTab, string url, bool multiThreadedMessageLoopEnabled)
        {
            InitializeComponent();

            var browser = new ChromiumWebBrowser(url)
            {
                Dock = DockStyle.Fill
            };

            browserPanel.Controls.Add(browser);

            Browser = browser;

            browser.MenuHandler = new MenuHandler();
            browser.RequestHandler = new WinFormsRequestHandler(openNewTab);
            browser.JsDialogHandler = new JsDialogHandler();
            browser.DownloadHandler = new DownloadHandler();
            browser.AudioHandler = new AudioHandler();

            if (multiThreadedMessageLoopEnabled)
            {
                browser.KeyboardHandler = new KeyboardHandler();
            }
            else
            {
                //When MultiThreadedMessageLoop is disabled we don't need the
                //CefSharp focus handler implementation.
                browser.FocusHandler = null;
            }

            //Handling DevTools docked inside the same window requires 
            //an instance of the LifeSpanHandler all the window events,
            //e.g. creation, resize, moving, closing etc.
            browser.LifeSpanHandler = new LifeSpanHandler(openPopupsAsTabs: false);

            browser.LoadingStateChanged += OnBrowserLoadingStateChanged;
            browser.ConsoleMessage += OnBrowserConsoleMessage;
            browser.TitleChanged += OnBrowserTitleChanged;
            browser.AddressChanged += OnBrowserAddressChanged;
            browser.StatusMessage += OnBrowserStatusMessage;
            browser.IsBrowserInitializedChanged += OnIsBrowserInitializedChanged;
            browser.LoadError += OnLoadError;

#if NETCOREAPP
            browser.JavascriptObjectRepository.Register("boundAsync", new AsyncBoundObject(), options: BindingOptions.DefaultBinder);
#else
            browser.JavascriptObjectRepository.Register("bound", new BoundObject(), isAsync: false, options: BindingOptions.DefaultBinder);
            browser.JavascriptObjectRepository.Register("boundAsync", new AsyncBoundObject(), isAsync: true, options: BindingOptions.DefaultBinder);
#endif

            //If you call CefSharp.BindObjectAsync in javascript and pass in the name of an object which is not yet
            //bound, then ResolveObject will be called, you can then register it
            browser.JavascriptObjectRepository.ResolveObject += (sender, e) =>
            {
                var repo = e.ObjectRepository;
                if (e.ObjectName == "boundAsync2")
                {
#if NETCOREAPP
                    repo.Register("boundAsync2", new AsyncBoundObject(), options: BindingOptions.DefaultBinder);
#else
                    repo.Register("boundAsync2", new AsyncBoundObject(), isAsync: true, options: BindingOptions.DefaultBinder);
#endif
                }
            };

            browser.RenderProcessMessageHandler = new RenderProcessMessageHandler();
            browser.DisplayHandler = new DisplayHandler();
            browser.MouseDown += (sender, args) => { Debug.WriteLine("MouseDown:" + args.X + ";" + args.Y + ";" + args.Button); };
            browser.MouseClick += (sender, args) => { Debug.WriteLine("MouseClick:" + args.X + ";" + args.Y + ";" + args.Button); };
            browser.MouseUp += (sender, args) => { Debug.WriteLine("MouseUp:" + args.X + ";" + args.Y + ";" + args.Button); };
            browser.MouseWheel += (sender, args) => { Debug.WriteLine("MouseWheel:" + args.X + ";" + args.Y + ";" + args.Button); };
            browser.MouseMove += (sender, args) => { Debug.WriteLine("MouseMove:" + args.X + ";" + args.Y + ";" + args.Button); };
            //browser.ResourceHandlerFactory = new FlashResourceHandlerFactory();
            this.multiThreadedMessageLoopEnabled = multiThreadedMessageLoopEnabled;

            var eventObject = new ScriptedMethodsBoundObject();
            eventObject.EventArrived += OnJavascriptEventArrived;
            // Use the default of camelCaseJavascriptNames
            // .Net methods starting with a capitol will be translated to starting with a lower case letter when called from js
#if !NETCOREAPP
            browser.JavascriptObjectRepository.Register("boundEvent", eventObject, isAsync: false, options: BindingOptions.DefaultBinder);
#endif

            CefExample.RegisterTestResources(browser);

            var version = string.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);
            //Set label directly, don't use DisplayOutput as call would be a NOOP (no valid handle yet).
            outputLabel.Text = version;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                    components = null;
                }
            }
            base.Dispose(disposing);
        }

        private void OnLoadError(object sender, LoadErrorEventArgs args)
        {
            //Don't display an error for external protocols that we allow the OS to
            //handle in OnProtocolExecution().
            if (args.ErrorCode == CefErrorCode.UnknownUrlScheme && args.Frame.Url.StartsWith("mailto"))
            {
                return;
            }

            DisplayOutput("Load Error:" + args.ErrorCode + ";" + args.ErrorText);
        }

        private void OnBrowserConsoleMessage(object sender, ConsoleMessageEventArgs args)
        {
            DisplayOutput(string.Format("Line: {0}, Source: {1}, Message: {2}", args.Line, args.Source, args.Message));
        }

        private void OnBrowserStatusMessage(object sender, StatusMessageEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => statusLabel.Text = args.Value);
        }

        private void OnBrowserLoadingStateChanged(object sender, LoadingStateChangedEventArgs args)
        {
            SetCanGoBack(args.CanGoBack);
            SetCanGoForward(args.CanGoForward);

            this.InvokeOnUiThreadIfRequired(() => SetIsLoading(args.IsLoading));
        }

        private void OnBrowserTitleChanged(object sender, TitleChangedEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => Parent.Text = args.Title);
        }

        private void OnBrowserAddressChanged(object sender, AddressChangedEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => urlTextBox.Text = args.Address);
        }

        private static void OnJavascriptEventArrived(string eventName, object eventData)
        {
            switch (eventName)
            {
                case "click":
                {
                    var message = eventData.ToString();
                    var dataDictionary = eventData as Dictionary<string, object>;
                    if (dataDictionary != null)
                    {
                        var result = string.Join(", ", dataDictionary.Select(pair => pair.Key + "=" + pair.Value));
                        message = "event data: " + result;
                    }
                    MessageBox.Show(message, "Javascript event arrived", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }
            }
        }

        private void SetCanGoBack(bool canGoBack)
        {
            this.InvokeOnUiThreadIfRequired(() => backButton.Enabled = canGoBack);
        }

        private void SetCanGoForward(bool canGoForward)
        {
            this.InvokeOnUiThreadIfRequired(() => forwardButton.Enabled = canGoForward);
        }

        private void SetIsLoading(bool isLoading)
        {
            goButton.Text = isLoading ?
                "Stop" :
                "Go";
            goButton.Image = isLoading ?
                Properties.Resources.nav_plain_red :
                Properties.Resources.nav_plain_green;

            HandleToolStripLayout();
        }

        private void OnIsBrowserInitializedChanged(object sender, EventArgs e)
        {
            //Get the underlying browser host wrapper
            var browserHost = Browser.GetBrowser().GetHost();
            var requestContext = browserHost.RequestContext;
            string errorMessage;
            // Browser must be initialized before getting/setting preferences
            var success = requestContext.SetPreference("enable_do_not_track", true, out errorMessage);
            if (!success)
            {
                this.InvokeOnUiThreadIfRequired(() => MessageBox.Show("Unable to set preference enable_do_not_track errorMessage: " + errorMessage));
            }

            //Example of disable spellchecking
            //success = requestContext.SetPreference("browser.enable_spellchecking", false, out errorMessage);

            var preferences = requestContext.GetAllPreferences(true);
            var doNotTrack = (bool)preferences["enable_do_not_track"];

            //Use this to check that settings preferences are working in your code
            //success = requestContext.SetPreference("webkit.webprefs.minimum_font_size", 24, out errorMessage);

            //Enable mouse events (experimental) so that MouseMove/MouseClick etc are triggered
            //Chromium by default doesn't propegate any mouse or keyboard messages, there is IKeyboardHandler
            //for capturing key events, unfortunately no mouse handler so we hook the low level hwnd to get the
            //message directly from Chromium and forward them to the ChromiumWebBrowser control
            this.BeginInvoke((Action)(() =>
            {
                //No action
                if (this.Disposing || this.IsDisposed || !this.IsHandleCreated)
                {
                    return;
                }

                ((ChromiumWebBrowser)Browser).EnableMouseEventsExperimental();
            }));            
        }

        private void DisplayOutput(string output)
        {
            outputLabel.InvokeOnUiThreadIfRequired(() => outputLabel.Text = output);
        }

        private void HandleToolStripLayout(object sender, LayoutEventArgs e)
        {
            HandleToolStripLayout();
        }

        private void HandleToolStripLayout()
        {
            var width = toolStrip1.Width;
            foreach (ToolStripItem item in toolStrip1.Items)
            {
                if (item != urlTextBox)
                {
                    width -= item.Width - item.Margin.Horizontal;
                }
            }
            urlTextBox.Width = Math.Max(0, width - urlTextBox.Margin.Horizontal - 18);
        }

        private void GoButtonClick(object sender, EventArgs e)
        {
            LoadUrl(urlTextBox.Text);
        }

        private void BackButtonClick(object sender, EventArgs e)
        {
            Browser.Back();
        }

        private void ForwardButtonClick(object sender, EventArgs e)
        {
            Browser.Forward();
        }

        private void UrlTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            LoadUrl(urlTextBox.Text);
        }

        private void LoadUrl(string url)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                Browser.Load(url);
            }
            else
            {
                var searchUrl = "https://www.google.com/search?q=" + Uri.EscapeDataString(url);

                Browser.Load(searchUrl);
            }

        }

        public async void CopySourceToClipBoardAsync()
        {
            var htmlSource = await Browser.GetSourceAsync();

            Clipboard.SetText(htmlSource);
            DisplayOutput("HTML Source copied to clipboard");
        }

        private void ToggleBottomToolStrip()
        {
            if (toolStrip2.Visible)
            {
                Browser.StopFinding(true);
                toolStrip2.Visible = false;
            }
            else
            {
                toolStrip2.Visible = true;
                findTextBox.Focus();
            }
        }

        private void FindNextButtonClick(object sender, EventArgs e)
        {
            Find(true);
        }

        private void FindPreviousButtonClick(object sender, EventArgs e)
        {
            Find(false);
        }

        private void Find(bool next)
        {
            if (!string.IsNullOrEmpty(findTextBox.Text))
            {
                Browser.Find(0, findTextBox.Text, next, false, false);
            }
        }

        private void FindTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            Find(true);
        }

        public void ShowFind()
        {
            ToggleBottomToolStrip();
        }

        private void FindCloseButtonClick(object sender, EventArgs e)
        {
            ToggleBottomToolStrip();
        }

        //Example of DevTools docked within the existing UserControl,
        //in this example it's hosted in a Panel with a SplitContainer
        public void ShowDevToolsDocked()
        {
            if (browserSplitContainer.Panel2Collapsed)
            {
                browserSplitContainer.Panel2Collapsed = false;
            }

            //Find devToolsControl in Controls collection
            DevToolsContainerControl devToolsControl = null;
            devToolsControl = browserSplitContainer.Panel2.Controls.Find(nameof(devToolsControl), false).FirstOrDefault() as DevToolsContainerControl;

            if (devToolsControl == null || devToolsControl.IsDisposed)
            {
                devToolsControl = new DevToolsContainerControl()
                {
                    Name = nameof(devToolsControl),
                    Dock = DockStyle.Fill
                };

                EventHandler devToolsPanelDisposedHandler = null;
                devToolsPanelDisposedHandler = (s, e) =>
                {
                    browserSplitContainer.Panel2.Controls.Remove(devToolsControl);
                    browserSplitContainer.Panel2Collapsed = true;
                    devToolsControl.Disposed -= devToolsPanelDisposedHandler;
                };

                //Subscribe for devToolsPanel dispose event
                devToolsControl.Disposed += devToolsPanelDisposedHandler;

                //Add new devToolsPanel instance to Controls collection
                browserSplitContainer.Panel2.Controls.Add(devToolsControl);
            }

            if (!devToolsControl.IsHandleCreated)
            {
                //It's very important the handle for the control is created prior to calling
                //SetAsChild, if the handle hasn't been created then manually call CreateControl();
                //This code is not required for this example, it's left here for demo purposes.
                devToolsControl.CreateControl();
            }

            //Devtools will be a child of the DevToolsContainerControl
            //DevToolsContainerControl is a simple custom Control that's only required
            //when CefSettings.MultiThreadedMessageLoop = false so arrow/tab key presses
            //are forwarded to DevTools correctly.
            var rect = devToolsControl.ClientRectangle;
            var windowInfo = new WindowInfo();
            windowInfo.SetAsChild(devToolsControl.Handle, rect.Left, rect.Top, rect.Right, rect.Bottom);
            Browser.GetBrowserHost().ShowDevTools(windowInfo);
        }

        public Task<bool> CheckIfDevToolsIsOpenAsync()
        {
            return Cef.UIThreadTaskFactory.StartNew(() =>
            {
                return Browser.GetBrowserHost().HasDevTools;
            });
        }
    }
}
