﻿// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "JavascriptObjectWrapper.h"

using namespace System;
using namespace CefSharp::Internals;

namespace CefSharp
{
    void JavascriptObjectWrapper::Bind(CefRefPtr<CefV8Value> v8Value, JavascriptCallbackRegistry^ callbackRegistry)
    {
        //Create property handler for get and set of Properties of this object
        _jsPropertyHandler = new JavascriptPropertyHandler(
            gcnew Func<String^, BrowserProcessResponse^>(this, &JavascriptObjectWrapper::GetProperty),
            gcnew Func<String^, Object^, BrowserProcessResponse^>(this, &JavascriptObjectWrapper::SetProperty)
            );

        //V8Value that represents this javascript object - only one per complex type
        auto javascriptObject = v8Value->CreateObject(_jsPropertyHandler.get());
        auto objectName = StringUtils::ToNative(_object->JavascriptName);
        v8Value->SetValue(objectName, javascriptObject, V8_PROPERTY_ATTRIBUTE_NONE);

        for each (JavascriptMethod^ method in Enumerable::OfType<JavascriptMethod^>(_object->Methods))
        {
            auto wrappedMethod = gcnew JavascriptMethodWrapper(method, _object->Id, _browserProcess, callbackRegistry);
            wrappedMethod->Bind(javascriptObject);

            _wrappedMethods->Add(wrappedMethod);
        }

        for each (JavascriptProperty^ prop in Enumerable::OfType<JavascriptProperty^>(_object->Properties))
        {
            auto wrappedproperty = gcnew JavascriptPropertyWrapper(prop, _object->Id, _browserProcess);
            wrappedproperty->Bind(javascriptObject, callbackRegistry);

            _wrappedProperties->Add(wrappedproperty);
        }
    }

    BrowserProcessResponse^ JavascriptObjectWrapper::GetProperty(String^ memberName)
    {
        auto task = _browserProcess->GetPropertyAsync(_object->Id, memberName);

        task->Wait();

        return task->Result;
    };

    BrowserProcessResponse^ JavascriptObjectWrapper::SetProperty(String^ memberName, Object^ value)
    {
        auto task = _browserProcess->SetPropertyAsync(_object->Id, memberName, value);

        task->Wait();

        return task->Result;
    };
}