// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    public class MessageHandlerBrowserSide : IDisposable
    {
        //TODO: Find proper home for this
        public static IManagedWrapperFactory WrapperFactory { get; set; }

        //contains in-progress eval script tasks
        private readonly PendingTaskRepository<JavascriptResponse> pendingTaskRepository;
        //contains js callback factories for each browser
        //Dictionary<int, IJavascriptCallbackFactory^>^ _javascriptCallbackFactories;
        private IBrowserAdapter browserAdapter;
        public IJavascriptCallbackFactory JavascriptCallbackFactory { get; private set; }

        public MessageHandlerBrowserSide(IBrowserAdapter browserAdapter)
        {
            this.browserAdapter = browserAdapter;
            JavascriptCallbackFactory = new JavascriptCallbackFactory(pendingTaskRepository);
            this.pendingTaskRepository = new PendingTaskRepository<JavascriptResponse>();
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
            //create a new taskcompletionsource
            var idAndComplectionSource = pendingTaskRepository.CreatePendingTask(timeout);

            var message = WrapperFactory.CreateProcessMessage(Messages.EvaluateJavascriptRequest);

            var argList = message.ArgumentList;
            argList.SetInt64(0, frameId);
            argList.SetInt64(1, idAndComplectionSource.Key);
            argList.SetString(2, script);

            browser.SendProcessMessage(ProcessId.Renderer, message);

            return idAndComplectionSource.Value.Task;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
