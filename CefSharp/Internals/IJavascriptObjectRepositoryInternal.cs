// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
// 

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    public interface IJavascriptObjectRepositoryInternal : IJavascriptObjectRepository
    {
        Task<Tuple<bool, object, string>> TryCallMethod(long objectId, string name, object[] parameters);
        bool TryGetProperty(long objectId, string name, out object result, out string exception);
        bool TrySetProperty(long objectId, string name, object value, out string exception);
        bool IsBrowserInitialized { get; set; }
        List<JavascriptObject> GetObjects(List<string> names = null);
        List<JavascriptObject> GetLegacyBoundObjects();
        void ObjectsBound(List<Tuple<string, bool, bool>> objs);
    }
}
