// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include\cef_v8.h"

#include "CefV8ValueWrapper.h"
#include "CefV8ContextWrapper.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    namespace Internals
    {
        private ref class JavascriptCallbackWrapper
        {
        private:
            CefV8ContextWrapper^ _context;
            CefV8ValueWrapper^ _value;

        public:
            JavascriptCallbackWrapper(CefV8ContextWrapper^ context, CefV8ValueWrapper^ value)
                : _context(context), _value(value)
            {
            }

            !JavascriptCallbackWrapper()
            {
                delete _context;
                delete _value;
                _context = nullptr;
                _value = nullptr;
            }

            ~JavascriptCallbackWrapper()
            {
                this->!JavascriptCallbackWrapper();
            }

            CefV8ContextWrapper^ GetContext()
            {
                return _context;
            }

            CefV8ValueWrapper^ GetValue()
            {
                return _value;
            }
        };
    }
}