// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"
#include "TypeUtils.h"

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

        bool IsSame(CefV8ValueWrapper^ other)
        {
            return _value->IsSame((CefRefPtr<CefV8Value>)other);
        }

        property bool IsValid
        {
            bool get()
            {
                return _value->IsValid();
            }
        }

        ///
        // True if the value type is undefined.
        ///
        /*--cef()--*/
        property bool IsUndefined
        {
            bool get()
            {
                return _value->IsUndefined();
            }
        }

        ///
        // True if the value type is null.
        ///
        /*--cef()--*/
        property bool IsNull
        {
            bool get()
            {
                return _value->IsNull();
            }
        }

        ///
        // True if the value type is bool.
        ///
        /*--cef()--*/
        property bool IsBool
        {
            bool get()
            {
                return _value->IsBool();
            }
        }

        ///
        // True if the value type is int.
        ///
        /*--cef()--*/
        property bool IsInt
        {
            bool get()
            {
                return _value->IsInt();
            }
        }

        property bool IsUInt
        {
            bool get()
            {
                return _value->IsUInt();
            }
        }

        property bool IsDouble
        {
            bool get()
            {
                return _value->IsDouble();
            }
        }

        property bool IsDate
        {
            bool get()
            {
                return _value->IsDate();
            }
        }

        property bool IsString
        {
            bool get()
            {
                return _value->IsString();
            }
        }

        property bool IsObject
        {
            bool get()
            {
                return _value->IsObject();
            }
        }

        property bool IsArray
        {
            bool get()
            {
                return _value->IsArray();
            }
        }

        property bool IsFunction
        {
            bool get()
            {
                return _value->IsFunction();
            }
        }

        bool GetBoolValue()
        {
            return _value->GetBoolValue();
        }

        int GetIntValue()
        {
            return _value->GetIntValue();
        }

        UInt32 GetUIntValue()
        {
            return _value->GetUIntValue();
        }

        double GetDoubleValue()
        {
            return _value->GetDoubleValue();
        }

        DateTime^ GetDateValue()
        {
            return TypeUtils::ConvertCefTimeToDateTime( _value->GetDateValue());
        }

        String^ GetStringValue()
        {
            return StringUtils::ToClr(_value->GetStringValue());
        }
    };
}
