// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"

#include "Stdafx.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    public ref class CefV8ValueWrapper
    {
    private:
        MCefRefPtr<CefV8Value> _value;
        
    public:
        CefV8ValueWrapper(CefRefPtr<CefV8Value> value)
        {
            _value = value;
        }
        
        !CefV8ValueWrapper()
        {
            _value = nullptr;
        }

        ~CefV8ValueWrapper()
        {
            this->!CefV8ValueWrapper();
        }

        operator CefRefPtr<CefV8Value>()
        {
            return _value.get();
        }
    };
}
