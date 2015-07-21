// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "CefV8ValueWrapper.h"
#include "BrowserProcessResponse.h"

using namespace System::Collections::Generic;
using namespace System::Threading::Tasks;

namespace CefSharp
{
    public interface class IBrowserProcess
    {
        Task<BrowserProcessResponse^>^ CallMethodAsync(int64 objectId, String^ name, IList<CefV8ValueWrapper^>^ parameters);
        Task<BrowserProcessResponse^>^ GetPropertyAsync(int64 objectId, String^ name);
        Task<BrowserProcessResponse^>^ SetPropertyAsync(int64 objectId, String^ name, Object^ value);
    };
}