﻿// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public class JavascriptResponse
    {
        public string Message { get; set; }

        public bool Success { get; set; }

        public object Result { get; set; }
    }
}
