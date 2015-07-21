// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.ServiceModel;
using CefSharp.Internals;
using CefSharp.BrowserSubprocess.Messaging;

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

        public override bool OnProcessMessageReceived(CefBrowserWrapper browser, IProcessMessage message)
        {
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

                var callbackId = argList.GetInt64(1, 2);
                var response = CreateProcessMessage(responseName);
                var responseArgList = response.ArgumentList;
                var errorMessage = String.Format("Request BrowserId : {0} not found it's likely the browser is already closed", browser.BrowserId);

                //success: false
                responseArgList.SetBool(0, false);
                responseArgList.SetInt64(1, 2, callbackId);
                responseArgList.SetString(3, errorMessage);
                browser.SendProcessMessage(response);

                return true;
            }

            if (name == Messages.RegisterJavascriptObjectsRequest)
            {
                browser.JavascriptRootObject = argList.DeserializeJsRootObject();

                return true;
            }

            if (name == Messages.EvaluateJavascriptRequest)
            {
                var frameId = argList.GetInt64(0, 1);
                var callbackId = argList.GetInt64(2, 3);
                var script = argList.GetString(4);
                var success = false;

                var response = CreateProcessMessage(Messages.EvaluateJavascriptResponse);
                var responseArgList = response.ArgumentList;

                var frame = browser.GetFrame(frameId);
                string errorMessage;
                if (frame != null)
                {
                    var context = frame.GetV8Context();

                    if (context == null)
                    {
                        errorMessage = "Unable to Enter Context";
                    }
                    else
                    {
                        CefV8ValueWrapper result;
                        success = context.Eval(script, out result, out errorMessage);

                        //we need to do this here to be able to store the v8context
                        if (success)
                        {
                            result.SerializeV8Object(responseArgList, 2, browser.CallbackRegistry, context);
                        }
                    }
                }
                else
                {
                    errorMessage = "Unable to Get Frame matching Id";
                }
                
                responseArgList.SetBool(0, success);
                responseArgList.SetInt64(1, 2, callbackId);
                if (!success)
                {
                    responseArgList.SetString(3, errorMessage);
                }
                browser.SendProcessMessage(response);

                return true;
            }
            else if (name == Messages.JavascriptCallbackRequest)
            {
                var jsCallbackId = argList.GetInt64(0, 1);
                var callbackId = argList.GetInt64(2, 3);

                var callbackRegistry = browser.CallbackRegistry;
                var callbackWrapper = callbackRegistry.FindWrapper(jsCallbackId);

                var parameterList = argList.GetList(4);
                var parameterListSize = (int)parameterList.GetSize();

                var paramList = new List<CefV8ValueWrapper>();
                for (var i = 0; i < parameterListSize; i++)
                {
                    paramList.Add(parameterList.DeserializeV8Object(i));
                }

                var response = CreateProcessMessage(Messages.JavascriptCallbackResponse);
                var responseArgList = response.ArgumentList;

                var context = callbackWrapper.GetContext();
                var value = callbackWrapper.GetValue();
                string errorMessage;
                var success = false;

                if (context != null && context.Enter())
                {
                    try
                    {
                        CefV8ValueWrapper result;
                        success = value.ExecuteFunction(paramList, out result, out errorMessage);

                        //we need to do this here to be able to store the v8context
                        if (success)
                        {
                            result.SerializeV8Object(responseArgList, 2, callbackRegistry, context);
                        }
                    }
                    finally
                    {
                        context.Exit();
                    }
                }
                else
                {
                    errorMessage = "Unable to Enter Context";
                }
                
                responseArgList.SetBool(0, success);
                responseArgList.SetInt64(1, 2, callbackId);
                responseArgList.SetString(3, errorMessage);

                browser.SendProcessMessage(response);

                return true;
            }
            else if (name == Messages.JavascriptCallbackDestroyRequest)
            {
                var jsCallbackId = argList.GetInt64(0, 1);
                browser.CallbackRegistry.Deregister(jsCallbackId);

                return true;
            }

            return false;
        }
    }
}
