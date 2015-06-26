﻿// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public static class CefSharpSettings
    {
        static CefSharpSettings()
        {
            WcfEnabled = true;
        }

        /// <summary>
        /// WCF is used by JavascriptBinding and EvaluateScriptAsync. 
        /// Disabling effectively disables both of these features.
        /// Defaults to true
        /// </summary>
        public static bool WcfEnabled { get; set; }
    }
}
