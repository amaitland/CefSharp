// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    public class JavascriptCallbackProxy : IJavascriptCallback
    {
        private readonly WeakReference browserWrapper;
        private JavascriptCallback callback;
        private PendingTaskRepository<JavascriptResponse> pendingTasks;
        private bool disposed;

        public JavascriptCallbackProxy(JavascriptCallback callback, PendingTaskRepository<JavascriptResponse> pendingTasks, WeakReference browserWrapper)
        {
            this.callback = callback;
            this.pendingTasks = pendingTasks;
            this.browserWrapper = browserWrapper;
        }

        public Task<JavascriptResponse> ExecuteAsync(object[] parameters)
        {
            DisposedGuard();

            return null;

            //auto browser = GetBrowser();
            //if (browser == nullptr)
            //{
            //	throw gcnew InvalidOperationException("Browser instance is null.");
            //}

            //auto doneCallback = _pendingTasks->CreatePendingTask(Nullable<TimeSpan>());
            //auto callbackMessage = CreateCallMessage(doneCallback.Key, parameters);
            //browser->SendProcessMessage(CefSharp.ProcessId::Renderer, gcnew CefProcessMessageWrapper(callbackMessage));

            //return doneCallback.Value->Task;
        }

        //IProcessMessage CreateCallMessage(int64 doneCallbackId, array<Object^>^ parameters)
        //{
        //	auto result = CefProcessMessage::Create(kJavascriptCallbackRequest);
        //	auto argList = result->GetArgumentList();
        //	SetInt64(_callback->Id, argList, 0);
        //	SetInt64(doneCallbackId, argList, 1);
        //	auto paramList = CefListValue::Create();
        //	for (int i = 0; i < parameters->Length; i++)
        //	{
        //		auto param = parameters[i];
        //		SerializeV8Object(param, paramList, i);
        //	}
        //	argList->SetList(2, paramList);
        //	return result;
        //}

        //IProcessMessage CreateDestroyMessage()
        //{
        //	auto result = CefProcessMessage::Create(kJavascriptCallbackDestroyRequest);
        //	auto argList = result->GetArgumentList();
        //	SetInt64(_callback->Id, argList, 0);
        //	return result;
        //}

        private IBrowser GetBrowser()
        {
            IBrowser result = null;
            if (browserWrapper.IsAlive)
            {
                result = (IBrowser)browserWrapper.Target;
            }
            return result;
        }

        private void DisposedGuard()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("JavascriptCallbackProxy");
            }
        }

        public void Dispose()
        {
            //var browser = GetBrowser();
            //if (browser != null && !browser.IsDisposed)
            //{
            //	browser->SendProcessMessage(CefSharp.ProcessId::Renderer, gcnew CefProcessMessageWrapper(CreateDestroyMessage()));
            //}
            disposed = true;
        }
    };
}
