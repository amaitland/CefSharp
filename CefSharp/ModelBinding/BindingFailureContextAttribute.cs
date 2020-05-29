// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.ModelBinding
{
    /// <summary>
    /// An attribute set on <see cref="BindingFailureCode"/> fields to provide context during an exception
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal class BindingFailureContextAttribute : Attribute
    {
        /// <summary>
        /// Create a new instance of <see cref="BindingFailureContextAttribute"/>
        /// </summary>
        /// <param name="context">A string that expands upon an error code. Helpful for debugging.</param>
        public BindingFailureContextAttribute(string context)
        {
            Value = context;
        }
        /// <summary>
        /// The context you're looking for.
        /// </summary>
        public string Value { get; }
    }
}
