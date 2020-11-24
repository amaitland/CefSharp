// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;

namespace CefSharp.JavascriptBinding
{
    /// <summary>
    /// Javascript Binding Method Invocation
    /// Details of the object and method to execute including
    /// optional list of paramaters
    /// </summary>
    public sealed class MethodInvocation
    {
        private readonly List<object> parameters = new List<object>();

        /// <summary>
        /// Browser Id
        /// </summary>
        public int BrowserId { get; private set; }

        /// <summary>
        /// Frame Id
        /// </summary>
        public long FrameId { get; private set; }

        /// <summary>
        /// Callback Id (Id of the object in the Render process) that will be used
        /// to transmit the result back to javascript engine
        /// </summary>
        public long? CallbackId { get; private set; }

        /// <summary>
        /// Id of the object stored in the <see cref="IJavascriptObjectRepository"/>
        /// </summary>
        public long ObjectId { get; private set; }

        /// <summary>
        /// .Net Method Name
        /// </summary>
        public string MethodName { get; private set; }

        /// <summary>
        /// List of params passed to the method invocation
        /// </summary>
        public List<object> Parameters
        {
            get { return parameters; }
        }

        /// <summary>
        /// MethodInvocation
        /// </summary>
        /// <param name="browserId">Browser Id</param>
        /// <param name="frameId">Frame Id</param>
        /// <param name="objectId">Id of the object stored in the <see cref="IJavascriptObjectRepository"/></param>
        /// <param name="methodName">.Net Method Name</param>
        /// <param name="callbackId">Callback Id (Id of the object in the Render process) that will be used
        /// to transmit the result back to javascript engine</param>
        public MethodInvocation(int browserId, long frameId, long objectId, string methodName, long? callbackId)
        {
            BrowserId = browserId;
            FrameId = frameId;
            CallbackId = callbackId;
            ObjectId = objectId;
            MethodName = methodName;
        }
    }
}
