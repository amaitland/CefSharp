// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.JavascriptBinding
{
    /// <summary>
    /// Result of a Javascript binding method Invocation
    /// </summary>
    public sealed class MethodInvocationResult
    {
        /// <summary>
        /// Browser Id
        /// </summary>
        public int BrowserId { get; set; }

        /// <summary>
        /// CallbackId
        /// </summary>
        public long? CallbackId { get; set; }

        /// <summary>
        /// Frame Id
        /// </summary>
        public long FrameId { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Success
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Result
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// Javascript Name converter
        /// </summary>
        public IJavascriptNameConverter NameConverter { get; set; }
    }
}
