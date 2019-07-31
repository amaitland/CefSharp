// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using CefSharp.Enums;

namespace CefSharp.Event
{
    /// <summary>
    /// Event arguments for the <see cref="IJavascriptObjectRepository.ResolveObject"/> event
    /// </summary>
    public class JavascriptBindingEventArgs : EventArgs
    {
        /// <summary>
        /// The javascript object repository, used to register objects
        /// </summary>
        public IJavascriptObjectRepository ObjectRepository { get; private set; }

        /// <summary>
        /// Name of the object
        /// </summary>
        public string ObjectName { get; private set; }

        /// <summary>
        /// Binding Strategy
        /// </summary>
        public JavascriptBindingStrategy Strategy { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objectRepository">object repository</param>
        /// <param name="strategy">strategy</param>
        /// <param name="name">object name</param>
        public JavascriptBindingEventArgs(IJavascriptObjectRepository objectRepository, JavascriptBindingStrategy strategy, string name)
        {
            ObjectRepository = objectRepository;
            Strategy = strategy;
            ObjectName = name;
        }
    }
}
