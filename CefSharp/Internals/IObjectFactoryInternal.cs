// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.JavascriptBinding;

namespace CefSharp.Internals
{
    /// <summary>
    /// Used to create objects that exist internally to the framework, allows for advanced
    /// users to replace some of the inner workings if required.
    /// </summary>
    public interface IObjectFactoryInternal
    {
        /// <summary>
        /// Create a new instance of the <see cref="IJavascriptObjectRepositoryInternal"/>
        /// Used for Javascript Binding.
        /// </summary>
        /// <returns>returns JavascriptObjectRepositry</returns>
        IJavascriptObjectRepositoryInternal CreateJavascriptObjectRepository();
        /// <summary>
        /// Create new instance of <see cref="IMethodRunnerQueue"/>
        /// Used for Javascript Binding.
        /// </summary>
        /// <returns>returns MethodRunnerQueue</returns>
        IMethodRunnerQueue CreateMethodRunnerQueue(IJavascriptObjectRepositoryInternal javascriptObjectRepository);
    }
}
