// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    //#define INTERNET_OPEN_TYPE_PRECONFIG                    0   // use registry configuration
    //#define INTERNET_OPEN_TYPE_DIRECT                       1   // direct to net
    //#define INTERNET_OPEN_TYPE_PROXY                        3   // via named proxy
    //#define INTERNET_OPEN_TYPE_PRECONFIG_WITH_NO_AUTOPROXY  4   // prevent using java/script/INS
    public enum ProxyType
    {
        /// <summary>
        /// use registry configuration
        /// </summary>
        PreConfig = 0,
        /// <summary>
        /// Direct to Net
        /// </summary>
        Direct = 1,
        /// <summary>
        /// Via Named Proxy
        /// </summary>
        Proxy = 3,
        /// <summary>
        /// prevent using java/script/INS
        /// </summary>
        PreConfigWithNoAutoProxy = 4
    }
}
