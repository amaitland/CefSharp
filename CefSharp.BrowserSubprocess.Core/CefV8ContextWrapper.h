// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"

#include "Stdafx.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    public ref class CefV8ContextWrapper
    {
    private:
        MCefRefPtr<CefV8Context> _context;
        
    public:
        CefV8ContextWrapper(CefRefPtr<CefV8Context> context)
        {
            _context = context;
        }
        
        !CefV8ContextWrapper()
        {
            _context = nullptr;
        }

        ~CefV8ContextWrapper()
        {
            this->!CefV8ContextWrapper();
        }

        operator CefRefPtr<CefV8Context>()
        {
            return _context.get();
        }

        bool Enter()
        {
            return _context->Enter();
        }

        bool Eval(String^ script)
        {
            //_context->Eval(
            return false;
        }
    };
}
