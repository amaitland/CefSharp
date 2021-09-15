// Copyright © 2011 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Net;
using System.Threading.Tasks;
using System.Windows;
using CefSharp.Example;
using CefSharp.Example.Handlers;
using CefSharp.Wpf.Example.Handlers;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace CefSharp.Wpf.Example
{
    public partial class App : Application
    {
        private ProxyServer proxyServer;

        private Task<bool> OnBasicAuth(SessionEventArgsBase ev, string u, string p)
        {
            if (u == "test" && p == "pass")
            {
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            proxyServer = new ProxyServer(userTrustRootCertificate: false);
            proxyServer.ProxyBasicAuthenticateFunc = OnBasicAuth;

            var explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Loopback, 8080, false);

            // An explicit endpoint is where the client knows about the existence of a proxy
            // So client sends request in a proxy friendly manner
            proxyServer.AddEndPoint(explicitEndPoint);
            proxyServer.Start();
            //System.AppContext.SetSwitch("Switch.System.Windows.Input.Stylus.DisableStylusAndTouchSupport", true);
            System.AppContext.SetSwitch("Switch.System.Windows.Input.Stylus.EnablePointerSupport", true);


#if DEBUG
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                MessageBox.Show("When running this Example outside of Visual Studio " +
                                "please make sure you compile in `Release` mode.", "Warning");
            }
#endif

            const bool multiThreadedMessageLoop = true;

            IBrowserProcessHandler browserProcessHandler;

            if (multiThreadedMessageLoop)
            {
                browserProcessHandler = new BrowserProcessHandler();
            }
            else
            {
                browserProcessHandler = new WpfBrowserProcessHandler(Dispatcher);
            }

            var settings = new CefSettings();
            settings.MultiThreadedMessageLoop = multiThreadedMessageLoop;
            settings.ExternalMessagePump = !multiThreadedMessageLoop;

            CefExample.Init(settings, browserProcessHandler: browserProcessHandler);

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            proxyServer?.Stop();
            proxyServer = null;

            base.OnExit(e);
        }
    }
}
