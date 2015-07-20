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
        CefV8ValueWrapper()
        {
            _value = CefV8Value::CreateNull();
        }

        CefV8ValueWrapper(bool value)
        {
            _value = CefV8Value::CreateBool(value);
        }

        CefV8ValueWrapper(int value)
        {
            _value = CefV8Value::CreateInt(value);
        }

        CefV8ValueWrapper(double value)
        {
            _value = CefV8Value::CreateDouble(value);
        }

        CefV8ValueWrapper(String^ value)
        {
            _value = CefV8Value::CreateString(StringUtils::ToNative(value));
        }

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

        bool ExecuteFunction(IList<CefV8ValueWrapper^>^ parameters, [Runtime::InteropServices::Out] CefV8ValueWrapper^ %resultWrapper, [Runtime::InteropServices::Out] String^ %errorMessage)
        {
            CefV8ValueList params;
            for (auto i = 0; i < parameters->Count; i++)
            {
                auto p = (CefRefPtr<CefV8Value>)parameters[i];
                params.push_back(p);
            }
            auto success = false;
            auto result = _value->ExecuteFunction(nullptr, params);

            success = result.get() != nullptr;
                        
            if (success)
            {
                resultWrapper = gcnew CefV8ValueWrapper(result);
            }
            else
            {
                auto exception = _value->GetException();
                if(exception.get())
                {
                    errorMessage = StringUtils::ToClr(exception->GetMessage());
                }
            }

            return success;
        }

        bool SetValue(int index, CefV8ValueWrapper^ value)
        {
            return _value->SetValue(index, value);
        }

        bool SetValue(String^ key, CefV8ValueWrapper^ value)
        {
            return _value->SetValue(StringUtils::ToNative(key), value, V8_PROPERTY_ATTRIBUTE_NONE);
        }

        static CefV8ValueWrapper^ CreateArray(int length)
        {
            return gcnew CefV8ValueWrapper(CefV8Value::CreateArray(length));
        }
    };
}
