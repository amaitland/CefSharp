﻿// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"
#include "JavascriptCallbackRegistry.h"
#include "BrowserProcessResponse.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    private class JavascriptMethodHandler : public CefV8Handler
    {
    private:
        gcroot<Func<IList<CefV8ValueWrapper^>^, BrowserProcessResponse^>^> _method;
        gcroot<JavascriptCallbackRegistry^> _callbackRegistry;

    public:
        JavascriptMethodHandler(Func<IList<CefV8ValueWrapper^>^, BrowserProcessResponse^>^ method, JavascriptCallbackRegistry^ callbackRegistry)
        {
            _method = method;
            _callbackRegistry = callbackRegistry;
        }

        ~JavascriptMethodHandler()
        {
            delete _method;
            delete _callbackRegistry;
        }

        virtual bool Execute(const CefString& name, CefRefPtr<CefV8Value> object, const CefV8ValueList& arguments, CefRefPtr<CefV8Value>& retval, CefString& exception);

        CefRefPtr<CefV8Value> ConvertToCefObject(Object^ obj);

        IMPLEMENT_REFCOUNTING(JavascriptMethodHandler)
    };
}