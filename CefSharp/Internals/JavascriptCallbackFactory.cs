// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Internals
{
    public class JavascriptCallbackFactory : IJavascriptCallbackFactory
    {
        private PendingTaskRepository<JavascriptResponse> pendingTasks;

        public JavascriptCallbackFactory(PendingTaskRepository<JavascriptResponse> pendingTasks)
        {
            this.pendingTasks = pendingTasks;
        }

        public WeakReference BrowserWrapper { get; set; }

        public IJavascriptCallback Create(JavascriptCallback callback)
        {
            return new JavascriptCallbackProxy(callback, pendingTasks, BrowserWrapper);
        }
    }
}
