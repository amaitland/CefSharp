// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.JavascriptBinding
{
    /// <summary>
    /// MethodInvocationComplete Event Args
    /// </summary>
    public sealed class MethodInvocationCompleteArgs : EventArgs
    {
        /// <summary>
        /// Invocation result
        /// </summary>
        public MethodInvocationResult Result { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="result">Invocation result</param>
        public MethodInvocationCompleteArgs(MethodInvocationResult result)
        {
            Result = result;
        }
    }
}
