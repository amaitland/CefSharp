// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.


namespace CefSharp.Internals
{
    /// <summary>
    /// Internal Implementation of <see cref="IObjectFactory"/>
    /// Factory methods are protected virtual and can be override to
    /// provide a custom implementation of an interface used internally
    /// within CefSharp.
    /// </summary>
    public class ObjectFactory : IObjectFactory
    {
        IMethodRunnerQueue IObjectFactory.CreateMethodRunnerQueue(IJavascriptObjectRepositoryInternal javascriptObjectRepository)
        {
            return CreateMethodRunnerQueue(javascriptObjectRepository);
        }

        /// <summary>
        /// Create a new instance of <see cref="IMethodRunnerQueue "/>
        /// </summary>
        /// <param name="javascriptObjectRepository">object repository</param>
        /// <returns>instance of the interface</returns>
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
