// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"

#include "Stdafx.h"

#include "CefV8ValueWrapper.h"
#include "CefV8ExceptionWrapper.h"

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

        bool Exit()
        {
            return _context->Exit();
        }

        bool Eval(String^ script, [Runtime::InteropServices::Out] CefV8ValueWrapper^ %value, [Runtime::InteropServices::Out] String^ %exception)
        {
            CefRefPtr<CefV8Value> retVal;
            CefRefPtr<CefV8Exception> retException;
            bool success = false;

            if (_context.get() && _context->Enter())
            {
                try
                {
                    success = _context->Eval(StringUtils::ToNative(script), retVal, retException);

                    if(!success)
                    {
                        exception = StringUtils::ToClr(retException->GetMessage());
                    }
                }
                finally
                {
                    _context->Exit();
                }
            }
            else
            {
                exception = "Unable to Enter Context";
            }

            value = gcnew CefV8ValueWrapper(retVal);

            return success;
        }
    };
}
