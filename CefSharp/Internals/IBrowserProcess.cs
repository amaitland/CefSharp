// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Internals
{
    public interface IBrowserProcess
    {
        BrowserProcessResponse CallMethod(long objectId, string name, object[] parameters);

        BrowserProcessResponse GetProperty(long objectId, string name);

        BrowserProcessResponse SetProperty(long objectId, string name, object value);
    }
}