// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;

namespace CefSharp.ModelBinding
{
    /// <summary>
    /// Base class provides common functionality for writing an
    /// async method interceptor
    /// </summary>
    public abstract class AsyncMethodInterceptorBase : IMethodInterceptor
    {
        object IMethodInterceptor.Intercept(Func<object[], object> method, object[] parameters, string methodName)
        {
            // run the asynchronous interceptor without worrying about creating a deadlock 
            var asyncIntercept = Task.Run(() => InterceptAsync(method, parameters, methodName));
            // blocks until the result is safely available on the current execution context. 
            return asyncIntercept.Result;
        }

        /// <summary>
        /// You are now responsible for evaluating the function and returning the result.
        /// </summary>
        /// <param name="method">A Func that represents the method to be called</param>
        /// <param name="parameters">parameters to be passed to <paramref name="method"/></param>
        /// <param name="methodName">Name of the method to be called</param>
        /// <returns>The method result</returns>
        /// <example>
        /// <see cref="TypeSafeInterceptor"/>
        /// </example>
        protected abstract Task<object> InterceptAsync(Func<object[], object> method, object[] parameters, string methodName);
    }
}
