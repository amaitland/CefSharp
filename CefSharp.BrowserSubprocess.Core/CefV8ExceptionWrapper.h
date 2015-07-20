// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"

#include "Stdafx.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    public ref class CefV8ExceptionWrapper
    {
    private:
        MCefRefPtr<CefV8Exception> _exception;
        
    public:
        CefV8ExceptionWrapper(CefRefPtr<CefV8Exception> exception)
        {
            _exception = exception;
        }
        
        !CefV8ExceptionWrapper()
        {
            _exception = nullptr;
        }

        ~CefV8ExceptionWrapper()
        {
            this->!CefV8ExceptionWrapper();
        }

        operator CefRefPtr<CefV8Exception>()
        {
            return _exception.get();
        }
    };
}
