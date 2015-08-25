﻿// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Linq;
using CefSharp.Internals;
using System.Collections.Generic;
using System.ServiceModel;

namespace CefSharp.BrowserSubprocess
{
    public class CefRenderProcess : CefSubProcess
    {
        public int? ParentProcessId { get; private set; }

        private int? parentBrowserId;
        private List<CefBrowserWrapper> browsers = new List<CefBrowserWrapper>();

        public CefRenderProcess(IEnumerable<string> args) 
        {
            LocateParentProcessId(args);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (browsers != null)
                {
                    foreach (var browser in browsers)
                    {
                        browser.Dispose();
                    }

                    browsers = null;
                }
            }

            base.Dispose(isDisposing);
        }

        public override void OnBrowserCreated(CefBrowserWrapper browser)
        {
            browsers.Add(browser);

            if (parentBrowserId == null)
            {
                parentBrowserId = browser.BrowserId;
            }

            if (ParentProcessId == null || parentBrowserId == null)
            {
                return;
            }

            var browserId = browser.IsPopup ? parentBrowserId.Value : browser.BrowserId;

            var serviceName = RenderprocessClientFactory.GetServiceName(ParentProcessId.Value, browserId);

            var binding = BrowserProcessServiceHost.CreateBinding();

            var channelFactory = new ChannelFactory<IBrowserProcess>(
                binding,
                new EndpointAddress(serviceName)
            );

            channelFactory.Open();

            var browserProcess = channelFactory.CreateChannel();
            var clientChannel = ((IClientChannel)browserProcess);

            try
            {
                clientChannel.Open();

                browser.ChannelFactory = channelFactory;
                browser.BrowserProcess = browserProcess;
            }
            catch(Exception)
            {
            }
        }

        public override void OnBrowserDestroyed(CefBrowserWrapper browser)
        {
            browsers.Remove(browser);

            var channelFactory = browser.ChannelFactory;

            if (channelFactory.State == CommunicationState.Opened)
            {
                channelFactory.Close();
            }

            var clientChannel = ((IClientChannel)browser.BrowserProcess);

            if (clientChannel.State == CommunicationState.Opened)
            {
                clientChannel.Close();
            }

            browser.ChannelFactory = null;
            browser.BrowserProcess = null;
        }

        private void LocateParentProcessId(IEnumerable<string> args)
        {
            // Format being parsed:
            // --channel=3828.2.1260352072\1102986608
            // We only really care about the PID (3828) part.
            const string channelPrefix = "--channel=";
            var channelArgument = args.SingleOrDefault(arg => arg.StartsWith(channelPrefix));
            if (channelArgument == null)
            {
                return;
            }

            var parentProcessId = channelArgument
                .Substring(channelPrefix.Length)
                .Split('.')
                .First();
            ParentProcessId = int.Parse(parentProcessId);
        }
    }
}
