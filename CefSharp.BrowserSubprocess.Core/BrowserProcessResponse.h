// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.


// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "CefV8ValueWrapper.h"

namespace CefSharp
{
    public ref class BrowserProcessResponse
    {
    public:
        property bool Success;
        property String^ Message;
        property CefV8ValueWrapper^ Result;
    };
}