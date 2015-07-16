// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    public class MessageHandlerBrowserSide
    {
        private IBrowserAdapter browserAdapter;
        private IJavascriptCallbackFactory javascriptCallbackFactory;

        public MessageHandlerBrowserSide(IBrowserAdapter browserAdapter, IJavascriptCallbackFactory javascriptCallbackFactory)
        {
            this.browserAdapter = browserAdapter;
            this.javascriptCallbackFactory = javascriptCallbackFactory;
        }

        public bool OnProcessMessageReceived(IBrowser browser, ProcessId sourceProcess, IProcessMessage message)
        {
            /*
             * 
             * auto handled = false;
            auto name = message->GetName();
            if (name == kEvaluateJavascriptResponse || name == kJavascriptCallbackResponse)
            {
                auto argList = message->GetArgumentList();
                auto success = argList->GetBool(0);
                auto callbackId = GetInt64(argList, 1);

                IJavascriptCallbackFactory^ callbackFactory;
                _javascriptCallbackFactories->TryGetValue(browser->GetIdentifier(), callbackFactory);

                auto pendingTask = _pendingTaskRepository->RemovePendingTask(callbackId);
                if (pendingTask != nullptr)
                {
                    auto response = gcnew JavascriptResponse();
                    response->Success = success;

                    if (success)
                    {
                        response->Result = DeserializeV8Object(argList, 2, callbackFactory);
                    }
                    else
                    {
                        response->Message = StringUtils::ToClr(argList->GetString(2));
                    }

                    pendingTask->SetResult(response);
                }

                handled = true;
            }

            return handled;
             */
            return false;
        }

        public Task<JavascriptResponse> EvaluateScriptAsync(IBrowser browser, Int64 frameId, String script, TimeSpan? timeout)
        {
            /*
             * //create a new taskcompletionsource
            auto idAndComplectionSource = _pendingTaskRepository->CreatePendingTask(timeout);

            auto message = CefProcessMessage::Create(kEvaluateJavascriptRequest);
            auto argList = message->GetArgumentList();
            SetInt64(frameId, argList, 0);
            SetInt64(idAndComplectionSource.Key, argList, 1);
            argList->SetString(2, StringUtils::ToNative(script));

            

            browserWrapper->SendProcessMessage(CefProcessId::PID_RENDERER, message);

            return idAndComplectionSource.Value->Task;
             */
            return null;
        }
    }
}
