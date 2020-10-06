// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Internals
{
    /// <summary>
    /// Simple factory used to create instances of objects CefSharp uses internally.
    /// At this point it's not nessicary to use a IoC container.
    /// </summary>
    public interface IObjectFactory
    {
        /// <summary>
        /// Create a new instance of <see cref="IMethodRunnerQueue "/>
        /// </summary>
        /// <param name="javascriptObjectRepository">object repository</param>
        /// <returns>instance of the interface</returns>
        IMethodRunnerQueue CreateMethodRunnerQueue(IJavascriptObjectRepositoryInternal javascriptObjectRepository);
    }
}
