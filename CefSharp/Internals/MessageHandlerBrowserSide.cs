// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    public class MessageHandlerBrowserSide : DisposableResource
    {
        //TODO: Find proper home for this
        public static IManagedWrapperFactory WrapperFactory { get; set; }

        //contains in-progress eval script tasks
        private readonly PendingTaskRepository<JavascriptResponse> pendingTaskRepository;
        private readonly JavascriptObjectRepository objectRepository;

        public MessageHandlerBrowserSide()
        {
            pendingTaskRepository = new PendingTaskRepository<JavascriptResponse>();
            objectRepository = new JavascriptObjectRepository();
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

            if (name == Messages.CallMethodRequest)
            {
                var i = 0;
                var argList = message.ArgumentList;
                var callBackId = argList.GetInt64(i++, i++);
                var objectId = argList.GetInt64(i++, i++);
                var methodName = argList.GetString(i++);
                var paramCount = argList.GetInt(i++);

                var parameters = new List<object>();

                for (var j = i; j < paramCount + i; j++)
                {
                    var obj = argList.DeserializeV8Object(j, new JavascriptCallbackFactory(pendingTaskRepository, browser));

                    parameters.Add(obj);
                }

                object result;
                string exception;
                var success = objectRepository.TryCallMethod(objectId, methodName, parameters.ToArray(), out result, out exception);

				var response = message.CreateResponse(Messages.CallMethodResponse);
	            var responseArgList = response.ArgumentList;
				responseArgList.SetInt64(0, 1, callBackId);
				responseArgList.SetBool(2, success);
	            if (success)
	            {
		            responseArgList.SetString(3, exception);
	            }
	            else
	            {
		            //responseArgList.SetList
	            }
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

        public void SendRegisteredJsObjects(IBrowser browser)
        {
            if (objectRepository.HasObjects)
            {
                var message = WrapperFactory.CreateProcessMessage(Messages.RegisterJavascriptObjectsRequest);
                message.ArgumentList.SerializeJsRootObject(objectRepository.RootObject);

                browser.SendProcessMessage(message);
            }
        }

        public void RegisterJsObject(string name, object obj, bool camelCaseJavascriptNames)
        {
            objectRepository.Register(name, obj, camelCaseJavascriptNames);
        }

        protected override void DoDispose(bool isDisposing)
        {
            //TODO: Implement
            base.DoDispose(isDisposing);
        }
    }
}
