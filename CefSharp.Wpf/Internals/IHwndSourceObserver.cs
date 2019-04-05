// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Interop;

namespace CefSharp.Wpf.Internals
{
    internal interface IHwndSourceObserver : IDisposable
    {
        void NotifySourceChange(HwndSource source);
    }
}
