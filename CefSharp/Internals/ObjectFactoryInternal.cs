// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.JavascriptBinding;

namespace CefSharp.Internals
{
    public class ObjectFactoryInternal : IObjectFactoryInternal
    {
        IJavascriptObjectRepositoryInternal IObjectFactoryInternal.CreateJavascriptObjectRepository()
        {
            return CreateJavascriptObjectRepository();
        }

        protected virtual IJavascriptObjectRepositoryInternal CreateJavascriptObjectRepository()
        {
            return new JavascriptObjectRepository();
        }

        IMethodRunnerQueue IObjectFactoryInternal.CreateMethodRunnerQueue(IJavascriptObjectRepositoryInternal javascriptObjectRepository)
        {
            return CreateMethodRunnerQueue(javascriptObjectRepository);
        }

        protected virtual IMethodRunnerQueue CreateMethodRunnerQueue(IJavascriptObjectRepositoryInternal javascriptObjectRepository)
        {
            if (CefSharpSettings.ConcurrentTaskExecution)
            {
                return new ConcurrentMethodRunnerQueue(javascriptObjectRepository);
            }

            return new MethodRunnerQueue(javascriptObjectRepository);
        }
    }
}
