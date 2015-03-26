// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public struct ProxyInfo
    {
        public ProxyType Type { get; set; }
        public string Address { get; set; }
        public string Bypass { get; set; }
    }
}
