// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// The referring-policy used for the navigation.
    /// </summary>
    public enum ReferrerPolicy
    {
        NoReferrer,
        NoReferrerWhenDowngrade,
        Origin,
        OriginWhenCrossOrigin,
        SameOrigin,
        StrictOrigin,
        StrictOriginWhenCrossOrigin,
        UnsafeUrl
    }
}