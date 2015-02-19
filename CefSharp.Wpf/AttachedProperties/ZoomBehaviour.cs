// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows;
using System.Windows.Threading;

namespace CefSharp.Wpf.AttachedProperties
{
    public static class ZoomBehaviour
    {
        public static readonly DependencyProperty AutoZoomProperty = DependencyProperty.RegisterAttached("AutoZoom", typeof(bool), typeof(ZoomBehaviour), new PropertyMetadata(false, OnAutoZoomChanged));

        public static bool GetAutoZoom(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoZoomProperty);
        }

        public static void SetAutoZoom(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoZoomProperty, value);
        }

        private static void OnAutoZoomChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var browser = (ChromiumWebBrowser)sender;

            var autoZoom = (bool)(e.NewValue);
            if (autoZoom)
            {
                browser.Unloaded += BrowserUnloaded;
                browser.FrameLoadStart += BrowserFrameLoadStart;
            }
            else
            {
                browser.Unloaded -= BrowserUnloaded;
                browser.FrameLoadStart -= BrowserFrameLoadStart;
            }
        }

        /// <summary>
        /// Browser Unloaded - Cleanup Event handlers
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event args</param>
        private static void BrowserUnloaded(object sender, RoutedEventArgs e)
        {
            var browser = (ChromiumWebBrowser)sender;

            browser.Unloaded -= BrowserUnloaded;
            browser.FrameLoadStart -= BrowserFrameLoadStart;
        }

        private static void BrowserFrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            var browser = (ChromiumWebBrowser)sender;

            browser.UiThreadRunAsync(() =>
            {
                var autoZoomLevel = browser.CalculateAutoZoom();
                browser.SetZoomLevel(autoZoomLevel);
            }, DispatcherPriority.Render);
        }
    }
}
