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
            var handled = false;
            var name = message.Name;
            if (name == Messages.EvaluateJavascriptResponse || name == Messages.JavascriptCallbackResponse)
            {
                var argList = message.ArgumentList;
                var success = argList.GetBool(0);
                var callbackId = argList.GetInt64(1);

                //IJavascriptCallbackFactory callbackFactory;
                //_javascriptCallbackFactories->TryGetValue(browser->GetIdentifier(), callbackFactory);

                var pendingTask = pendingTaskRepository.RemovePendingTask(callbackId);
                if (pendingTask != null)
                {
                    var response = new JavascriptResponse
                    {
                        Success = success
                    };

                    if (success)
                    {
                        response.Result = DeserializeV8Object(argList, 2, null);
                    }
                    else
                    {
                        response.Message = argList.GetString(2);
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

        //TODO: Turn this into an extension method
        private object DeserializeV8Object(IListValue list, int index, IJavascriptCallbackFactory javascriptCallbackFactory)
        {
            var type = list.GetType(index);

            if (type == CefValueType.Bool)
            {
                return list.GetBool(index);
            }
            
            if (type == CefValueType.Int)
            {
                return list.GetInt(index);
            }
            
            if (type == CefValueType.Double)
            {
                return list.GetDouble(index);
            }

            if (type == CefValueType.String)
            {
                return list.GetString(index);
            }

            if (type == CefValueType.List)
            {
                var subList = list.GetList(index);
                var array = new List<Object>((int)subList.GetSize());
                for (var i = 0; i < array.Capacity; i++)
                {
                    array.Add(DeserializeV8Object(subList, i, javascriptCallbackFactory));
                }
                return array;
            }

            if (type == CefValueType.Dictionary)
            {
                throw new NotImplementedException();
                //var dict = new Dictionary<String, Object>();
                //var subDict = list.GetDictionary(index);
                //std::vector<CefString> keys;
                //subDict.GetKeys(keys);

                //for (auto i = 0; i < keys.size(); i++)
                //{
                //	dict->Add(StringUtils::ToClr(keys[i]), DeserializeV8Object(subDict, keys[i], javascriptCallbackFactory));
                //}

                //result = dict;
            }

            if (type == CefValueType.Binary)
            {
                var binary = list.GetBinary(index);

                var t = (PrimitiveType)binary[0];

                if (t == PrimitiveType.Int64)
                {
                    return BitConverter.ToInt64(binary, 1);
                }

                if (t == PrimitiveType.CefTime)
                {
                    var epoch = BitConverter.ToDouble(binary, 1);

                    return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(epoch).ToLocalTime();
                }

                if (t == PrimitiveType.JsCallback)
                {
                    var browserId = new byte[sizeof(int)];

                    //Copy the middle bytes out that represent Browser Id
                    Array.Copy(binary, 1, browserId, 0, browserId.Length);

                    var result = new JavascriptCallback
                    {
                        BrowserId = BitConverter.ToInt32(browserId, 0),
                        Id = BitConverter.ToInt64(binary, 1 + sizeof (int))
                    };

                    return javascriptCallbackFactory.Create(result);
                }
            }

            throw new NotImplementedException();
        }
    }
}
