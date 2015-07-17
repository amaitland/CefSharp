// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Internals
{
    public class JavascriptCallbackFactory : IJavascriptCallbackFactory
    {
        private readonly PendingTaskRepository<JavascriptResponse> pendingTasks;
        private readonly WeakReference browser;

        public JavascriptCallbackFactory(PendingTaskRepository<JavascriptResponse> pendingTasks, IBrowser browser)
        {
            this.pendingTasks = pendingTasks;
            this.browser = new WeakReference(browser);
        }

        public IJavascriptCallback Create(JavascriptCallback callback)
        {
            return new JavascriptCallbackProxy(callback, pendingTasks, browser);
        }
    }
}
