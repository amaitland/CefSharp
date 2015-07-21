﻿// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    public class MessageHandlerBrowserSide : DisposableResource
    {
        //TODO: Find proper home for this
        public static IManagedWrapperFactory WrapperFactory { get; set; }

        //contains in-progress eval script tasks
        private readonly PendingTaskRepository<JavascriptResponse> pendingTaskRepository;

        public MessageHandlerBrowserSide()
        {
            pendingTaskRepository = new PendingTaskRepository<JavascriptResponse>();
        }

        public bool OnProcessMessageReceived(IBrowser browser, IProcessMessage message)
        {
            var handled = false;
            var name = message.Name;
            if (name == Messages.EvaluateJavascriptResponse || name == Messages.JavascriptCallbackResponse)
            {
                var argList = message.ArgumentList;
                var success = argList.GetBool(0);
                var callbackId = argList.GetInt64(1, 2);

                var pendingTask = pendingTaskRepository.RemovePendingTask(callbackId);
                if (pendingTask != null)
                {
                    var response = new JavascriptResponse
                    {
                        Success = success
                    };

                    if (success)
                    {
                        response.Result = argList.DeserializeV8Object(3, new JavascriptCallbackFactory(pendingTaskRepository, browser));
                    }
                    else
                    {
                        response.Message = argList.GetString(3);
                    }

                    pendingTask.SetResult(response);
                }

                handled = true;
            }

            return handled;
        }

        public Task<JavascriptResponse> EvaluateScriptAsync(IBrowser browser, Int64 frameId, String script, TimeSpan? timeout)
        {
            //create a new taskcompletionsource
            var idAndComplectionSource = pendingTaskRepository.CreatePendingTask(timeout);

            var message = WrapperFactory.CreateProcessMessage(Messages.EvaluateJavascriptRequest);

            var argList = message.ArgumentList;
            argList.SetInt64(0, 1, frameId);
            argList.SetInt64(2, 3, idAndComplectionSource.Key);
            argList.SetString(4, script);

            browser.SendProcessMessage(message);

            return idAndComplectionSource.Value.Task;
        }

        protected override void DoDispose(bool isDisposing)
        {
            //TODO: Implement
            base.DoDispose(isDisposing);
        }
    }
}
