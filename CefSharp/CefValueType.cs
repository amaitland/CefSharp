// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.using System;

namespace CefSharp
{
    public enum CefValueType
    {
        Invalid = 0,
        Null,
        Bool,
        Int,
        Double,
        String,
        Binary,
        Dictionary,
        List
    }
}
