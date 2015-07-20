// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_frame.h"

#include "Stdafx.h"
#include "CefV8ContextWrapper.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    public ref class CefFrameWrapper
    {
    private:
        MCefRefPtr<CefFrame> _frame;
        
    public:
        CefFrameWrapper(CefRefPtr<CefFrame> frame)
        {
            _frame = frame;
        }
        
        !CefFrameWrapper()
        {
            _frame = nullptr;
        }

        ~CefFrameWrapper()
        {
            this->!CefFrameWrapper();
        }

        operator CefRefPtr<CefFrame>()
        {
            return _frame.get();
        }

        CefV8ContextWrapper^ GetV8Context()
        {
            auto context = _frame->GetV8Context();
            if(context.get())
            {
                return gcnew CefV8ContextWrapper(context);
            }

            return nullptr;
        }
    };
}
