// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using CefSharp.Internals;
using System.Collections.Generic;
using System.ServiceModel;

namespace CefSharp.BrowserSubprocess
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults=true)]
    public class CefRenderProcess : CefSubProcess
    {
        private readonly bool wcfEnabled;
        private int? parentBrowserId;
        private List<CefBrowserWrapper> browsers = new List<CefBrowserWrapper>();

        public CefRenderProcess(IEnumerable<string> args, bool wcfEnabled) 
            : base(args)
        {
            this.wcfEnabled = wcfEnabled;
        }
        
        protected override void DoDispose(bool isDisposing)
        {
            foreach(var browser in browsers)
            {
                browser.Dispose();
            }

            browsers = null;

            base.DoDispose(isDisposing);
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

            //NOTE: Short term solution whilst we rewrite the IPC in stages
            if (!wcfEnabled)
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
                if (!browser.IsPopup)
                {
                    browserProcess.Connect();
                }

                var javascriptObject = browserProcess.GetRegisteredJavascriptObjects();

                if (javascriptObject.MemberObjects.Count > 0)
                {
                    browser.JavascriptRootObject = javascriptObject;
                }

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

            //NOTE: Short term solution whilst we rewrite the IPC in stages
            if (!wcfEnabled)
            {
                return;
            }

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
            browser.JavascriptRootObject = null;
        }

        public override bool OnProcessMessageReceived(CefBrowserWrapper browser, ProcessId sourceProcessId, IProcessMessage message)
        {
            var handled = false;
            var name = message.Name;
            var argList = message.ArgumentList;

            //Error handling for missing/closed browser
            if (browser == null)
            {
                if (name == Messages.JavascriptCallbackDestroyRequest)
                {
                    //If we can't find the browser wrapper then we'll just
                    //ignore this as it's likely already been disposed of
                    return true;
                }

                string responseName;
                if (name == Messages.EvaluateJavascriptRequest)
                {
                    responseName = Messages.EvaluateJavascriptResponse;
                }
                else if (name == Messages.JavascriptCallbackRequest)
                {
                    responseName = Messages.JavascriptCallbackResponse;
                }
                else
                {
                    //TODO: Should be throw an exception here? It's likely that only a CefSharp developer would see this
                    // when they added a new message and havn't yet implemented the render process functionality.
                    throw new Exception("Unsupported message type");
                }

                var callbackId = argList.GetInt64(1);
                var response = CreateProcessMessage(responseName);
                var responseArgList = response.ArgumentList;
                var errorMessage = String.Format("Request BrowserId : {0} not found it's likely the browser is already closed", browser.BrowserId);

                //success: false
                responseArgList.SetBool(0, false);
                responseArgList.SetInt64(1, callbackId);
                responseArgList.SetString(2, errorMessage);
                browser.SendProcessMessage(sourceProcessId, response);

                return true;
            }

            //these messages are roughly handled the same way
            if (name == Messages.EvaluateJavascriptRequest || name == Messages.JavascriptCallbackRequest)
            {
                var success = false;
                //CefRefPtr<CefV8Value> result;
                var errorMessage = "";
                IProcessMessage response;
                //both messages have the callbackid stored at index 1
                var callbackId = argList.GetInt64(1);

                if (name == Messages.EvaluateJavascriptRequest)
                {
                    var frameId = argList.GetInt64(0);
                    var script = argList.GetString(2);

                    response = CreateProcessMessage(Messages.EvaluateJavascriptResponse);

                    //var frame = browser.GetFrame(frameId);
                    //if (frame.get())
                    //{
                    //	var context = frame.GetV8Context();

                    //	if (context.get() && context.Enter())
                    //	{
                    //		try
                    //		{
                    //			CefRefPtr<CefV8Exception> exception;
                    //			success = context.Eval(script, result, exception);

                    //			//we need to do this here to be able to store the v8context
                    //			if (success)
                    //			{
                    //				var responseArgList = response.GetArgumentList();
                    //				SerializeV8Object(result, responseArgList, 2, browserWrapper.CallbackRegistry);
                    //			}
                    //			else
                    //			{
                    //				errorMessage = exception.GetMessage();
                    //			}
                    //		}
                    //		finally
                    //		{
                    //			context.Exit();
                    //		}
                    //	}
                    //	else
                    //	{
                    //		errorMessage = "Unable to Enter Context";
                    //	}
                    //}
                    //else
                    //{
                    //	errorMessage = "Unable to Get Frame matching Id";
                    //}
                }
                else
                {
                    var jsCallbackId = argList.GetInt64(0);
                    
                    var callbackRegistry = browser.CallbackRegistry;
                    var callbackWrapper = callbackRegistry.FindWrapper(jsCallbackId);

                    response = callbackWrapper.Execute(message, callbackRegistry);
                }
                
                browser.SendProcessMessage(sourceProcessId, response);

                handled = true;
            }
            else if (name == Messages.JavascriptCallbackDestroyRequest)
            {
                var jsCallbackId = argList.GetInt64(0);
                browser.CallbackRegistry.Deregister(jsCallbackId);

                handled = true;
            }

            return handled;
        }
    }
}
