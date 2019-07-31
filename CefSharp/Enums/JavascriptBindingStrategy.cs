// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Enums
{
    public enum JavascriptBindingStrategy
    {
        /// <summary>
        /// The default is to use CefSharp.BindObjectAsync to bind the object
        /// in javascript
        /// </summary>
        Default = 0,
        /// <summary>
        /// Legacy behavour - objects are immediately created in all Javascript V8
        /// Contexts (objects are registered in popups as well).
        /// Same as used in version 57.0.0 and below.
        /// </summary>
        Legacy = 1
    }
}
