// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows;

namespace CefSharp.Wpf.Example.Handlers
{
    public class LifespanHandler : ILifeSpanHandler
    {
        private readonly Window owner;

        public LifespanHandler(Window owner)
        {
            this.owner = owner;
        }

        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, ref int x, ref int y, ref int width, ref int height, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            ChromiumWebBrowser chromiumBrowser = null;

            var windowX = (x == int.MinValue) ? double.NaN : x;
            var windowY = (y == int.MinValue) ? double.NaN : y;
            var windowWidth = (width == int.MinValue) ? double.NaN : width;
            var windowHeight = (height == int.MinValue) ? double.NaN : height;

            owner.Dispatcher.Invoke(() =>
            {
                chromiumBrowser = new ChromiumWebBrowser()
                {
                    Address = targetUrl,
                };

                var popup = new Window
                {
                    Left = windowX,
                    Top = windowY,
                    Width = windowWidth,
                    Height = windowHeight,
                    Content = chromiumBrowser,
                    Owner = owner,
                    Title = targetFrameName
                };

                popup.Closed += (o, e) => 
                {
                    var w = o as Window;
                    if (w != null && w.Content is IWebBrowser)
                    {
                        (w.Content as IWebBrowser).Dispose();
                        w.Content = null;
                    }
                };

                chromiumBrowser.LifeSpanHandler = new LifespanHandler(popup);
            });

            newBrowser = chromiumBrowser;

            return false;
        }

        public void OnAfterCreated(IWebBrowser browser)
        {
            owner.Dispatcher.Invoke(() =>
            {
                if (owner != null && owner.Content == browser && !(owner is MainWindow))
                {
                    owner.Show();
                }
            });
        }

        public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        {
            owner.Dispatcher.Invoke(() =>
            {
                if (owner != null && owner.Content == browser)
                {
                    if (!(owner is MainWindow))
                    {
                        owner.Close();
                    }                        
                }
            });
        }
    }
}
