// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "CefListValueWrapper.h"

namespace CefSharp
{
    public ref class CefDictionaryValueWrapper : public IDictionaryValue
    {
    private:
        MCefRefPtr<CefDictionaryValue> _dictionaryValue;

    public:
        CefDictionaryValueWrapper(CefRefPtr<CefDictionaryValue> &dictionaryValue) : _dictionaryValue(dictionaryValue)
        {
            
        }

        !CefDictionaryValueWrapper()
        {
            _dictionaryValue = NULL;
        }

        ~CefDictionaryValueWrapper()
        {
            this->!CefDictionaryValueWrapper();
        }

        ///
        // Returns true if this object is valid. Do not call any other methods if this
        // method returns false.
        ///
        /*--cef()--*/
        virtual property bool IsValid
        {
            bool get()
            {
                return _dictionaryValue->IsValid();
            }
        }

        ///
        // Returns true if this object is currently owned by another object.
        ///
        /*--cef()--*/
        virtual property bool IsOwned
        {
            bool get()
            {
                return _dictionaryValue->IsOwned();
            }
        }

        ///
        // Returns true if the values of this object are read-only. Some APIs may
        // expose read-only objects.
        ///
        /*--cef()--*/
        virtual property bool IsReadOnly
        {
            bool get()
            {
                return _dictionaryValue->IsReadOnly();
            }
        }

        ///
        // Returns a writable copy of this object.
        ///
        /*--cef()--*/
        virtual IDictionaryValue^ Copy()
        {
        }

        ///
        // Removes all values. Returns true on success.
        ///
        /*--cef()--*/
        virtual bool Clear()
        {
            _dictionaryValue->Clear();
        }

        ///
        // Removes the value at the specified index.
        ///
        /*--cef(index_param=index)--*/
        virtual bool Remove(String^ key)
        {
            return _dictionaryValue->Remove(StringUtils::ToNative(key));
        }

        ///
        // Returns the value type at the specified index.
        ///
        /*--cef(default_retval=VTYPE_INVALID,index_param=index)--*/
        virtual CefSharp::Internals::CefValueType GetType(String^ key)
        {
            return _dictionaryValue->GetType(StringUtils::ToNative(key));
        }

        ///
        // Returns the value at the specified index as type bool.
        ///
        /*--cef(index_param=index)--*/
        virtual bool GetBool(String^ key)
        {
            return _dictionaryValue->GetBool(StringUtils::ToNative(key));
        }

        ///
        // Returns the value at the specified index as type int.
        ///
        /*--cef(index_param=index)--*/
        virtual int GetInt(String^ key)
        {
            return _dictionaryValue->GetInt(StringUtils::ToNative(key));
        }

        ///
        // Returns the value at the specified index as type double.
        ///
        /*--cef(index_param=index)--*/
        virtual double GetDouble(String^ key)
        {
            return _dictionaryValue->GetDouble(StringUtils::ToNative(key));
        }

        ///
        // Returns the value at the specified index as type string.
        ///
        /*--cef(index_param=index)--*/
        virtual String^ GetString(String^ key)
        {
            return StringUtils::ToClr(_dictionaryValue->GetString(StringUtils::ToNative(key)));
        }

        ///
        // Returns the value at the specified index as type binary.
        ///
        /*--cef(index_param=index)--*/
        virtual array<Byte>^ GetBinary(String^ key)
        {
            auto binary = _dictionaryValue->GetBinary(StringUtils::ToNative(key));
            auto byteCount = binary->GetSize();
            if (byteCount == 0)
            {
                return nullptr;
            }

            auto bytes = gcnew array<Byte>(byteCount);
            pin_ptr<Byte> src = &bytes[0]; // pin pointer to first element in arr

            binary->GetData(static_cast<void*>(src), byteCount, 0);

            return bytes;
        }

        ///
        // Returns the value at the specified index as type dictionary.
        ///
        /*--cef(index_param=index)--*/
        virtual IDictionaryValue^ GetDictionary(String^ key)
        {
            return gcnew CefDictionaryValueWrapper(_dictionaryValue->GetDictionary(StringUtils::ToNative(key)));
        }

        ///
        // Returns the value at the specified index as type list.
        ///
        /*--cef(index_param=index)--*/
        virtual IListValue^ GetList(String^ key)
        {
            return gcnew CefListValueWrapper(_dictionaryValue->GetList(StringUtils::ToNative(key)));
        }

        ///
        // Sets the value at the specified index as type null. Returns true if the
        // value was set successfully.
        ///
        /*--cef(index_param=index)--*/
        virtual bool SetNull(String^ key)
        {
            return _dictionaryValue->SetNull(StringUtils::ToNative(key));
        }

        ///
        // Sets the value at the specified index as type bool. Returns true if the
        // value was set successfully.
        ///
        /*--cef(index_param=index)--*/
        virtual bool SetBool(String^ key, bool value)
        {
            return _dictionaryValue->SetBool(StringUtils::ToNative(key), value);
        }

        ///
        // Sets the value at the specified index as type int. Returns true if the
        // value was set successfully.
        ///
        /*--cef(index_param=index)--*/
        virtual bool SetInt(String^ key, int value)
        {
            return _dictionaryValue->SetInt(StringUtils::ToNative(key), value);
        }

        ///
        // Sets the value at the specified index as type double. Returns true if the
        // value was set successfully.
        ///
        /*--cef(index_param=index)--*/
        virtual bool SetDouble(String^ key, double value)
        {
            return _dictionaryValue->SetDouble(StringUtils::ToNative(key), value);
        }

        ///
        // Sets the value at the specified index as type string. Returns true if the
        // value was set successfully.
        ///
        /*--cef(optional_param=value,index_param=index)--*/
        virtual bool SetString(String^ key, string value)
        {
            return _dictionaryValue->SetString(StringUtils::ToNative(key), value);
        }

        ///
        // Sets the value at the specified index as type binary. Returns true if the
        // value was set successfully. After calling this method the |value| object
        // will no longer be valid. If |value| is currently owned by another object
        // then the value will be copied and the |value| reference will not change.
        // Otherwise, ownership will be transferred to this object and the |value|
        // reference will be invalidated.
        ///
        /*--cef(index_param=index)--*/
        virtual bool SetBinary(String^ key, array<Byte>^ value)
        {
            pin_ptr<Byte> src = &value[0];

            auto binary = CefBinaryValue::Create(static_cast<void*>(src), value->Length);
            return _dictionaryValue->SetBinary(StringUtils::ToNative(key), binary);
        }

        ///
        // Sets the value at the specified index as type dict. Returns true if the
        // value was set successfully. After calling this method the |value| object
        // will no longer be valid. If |value| is currently owned by another object
        // then the value will be copied and the |value| reference will not change.
        // Otherwise, ownership will be transferred to this object and the |value|
        // reference will be invalidated.
        ///
        /*--cef(index_param=index)--*/
        virtual bool SetDictionary(String^ key, IDictionaryValue^ value)
        {
            auto dictionary = (CefDictionaryValueWrapper^)value;
            return _dictionaryValue->SetDictionary(StringUtils::ToNative(key), dictionary->GetDictionaryValue().get());
        }

        ///
        // Sets the value at the specified index as type list. Returns true if the
        // value was set successfully. After calling this method the |value| object
        // will no longer be valid. If |value| is currently owned by another object
        // then the value will be copied and the |value| reference will not change.
        // Otherwise, ownership will be transferred to this object and the |value|
        // reference will be invalidated.
        ///
        /*--cef(index_param=index)--*/
        virtual bool SetList(String^ key, IList<IListValue^>^ value)
        {
            auto list = (CefListValueWrapper^)value;
            return _dictionaryValue->SetList(StringUtils::ToNative(key), list->GetListValue().get());
        }

        MCefRefPtr<CefDictionaryValue> GetDictionaryValue()
        {
            return _dictionaryValue;
        }
    };
}