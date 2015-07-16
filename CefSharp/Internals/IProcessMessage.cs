// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Internals
{
    public interface IProcessMessage : IDisposable
    {
        string Name { get; }
        bool IsReadOnly { get; }
        bool IsValid { get; }
        IListValue ArgumentList { get; }
    }
}
