// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.


using System;
using System.Reflection;
using System.Windows.Input;

namespace CefSharp.Wpf.Experimental
{
    public static class WmPointerWebBrowserExtensions
    {
        //https://github.com/jaytwo/WmTouchDevice
        //https://docs.microsoft.com/en-gb/windows/desktop/wintouch/getting-started-with-multi-touch-messages
        public static void EnableExperimentalTouchSupport(this ChromiumWebBrowser browser)
        {
            var osVer = Environment.OSVersion;
            var ver = osVer.Version.Major + osVer.Version.Minor;
            if (ver < 8.1)
            {
                throw new NotSupportedException("This experimental feature is for Windows 8.1 and above only.");
            }

            DisableWPFTabletSupport();

            browser.RegisterHwndSourceObserver(new WmPointerHandler(browser));
        }

        /// <summary>
        /// https://docs.microsoft.com/en-us/dotnet/framework/wpf/advanced/disable-the-realtimestylus-for-wpf-applications
        /// </summary>
        private static void DisableWPFTabletSupport()
        {
            // Get a collection of the tablet devices for this window.    
            var devices = Tablet.TabletDevices;

            if (devices.Count > 0)
            {
                // Get the Type of InputManager.  
                var inputManagerType = typeof(InputManager);

                // Call the StylusLogic method on the InputManager.Current instance.  
                object stylusLogic = inputManagerType.InvokeMember("StylusLogic",
                            BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                            null, InputManager.Current, null);

                if (stylusLogic != null)
                {
                    //  Get the type of the stylusLogic returned from the call to StylusLogic.  
                    var stylusLogicType = stylusLogic.GetType();

                    // Loop until there are no more devices to remove.  
                    while (devices.Count > 0)
                    {
                        // Remove the first tablet device in the devices collection.  
                        stylusLogicType.InvokeMember("OnTabletRemoved",
                                BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic,
                                null, stylusLogic, new object[] { (uint)0 });
                    }
                }

            }
        }
    }
}
