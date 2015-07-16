// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "CefDictionaryValueWrapper.h"

namespace CefSharp
{
    public ref class CefListValueWrapper : public IListValue
    {
    private:
        MCefRefPtr<CefListValue> _listValue;

    public:
        CefListValueWrapper(CefRefPtr<CefListValue> &listValue) : _listValue(listValue)
        {
            
        }

        !CefListValueWrapper()
        {
            _listValue = NULL;
        }

        ~CefListValueWrapper()
        {
            this->!CefListValueWrapper();
        }

        operator CefRefPtr<CefListValue>()
        {
            return _listValue.get();
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
                return _listValue->IsValid();
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
                return _listValue->IsOwned();
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
                return _listValue->IsReadOnly();
            }
        }

        ///
        // Returns a writable copy of this object.
        ///
        /*--cef()--*/
        virtual IListValue^ Copy()
        {
            throw gcnew NotImplementedException();
        }

        ///
        // Sets the number of values. If the number of values is expanded all
        // new value slots will default to type null. Returns true on success.
        ///
        /*--cef()--*/
        virtual bool SetSize(UIntPtr size)
        {
            return _listValue->SetSize((size_t)size);
        }

        ///
        // Returns the number of values.
        ///
        /*--cef()--*/
        virtual UIntPtr GetSize()
        {
            return (UIntPtr)_listValue->GetSize();
        }

        ///
        // Removes all values. Returns true on success.
        ///
        /*--cef()--*/
        virtual bool Clear()
        {
            return _listValue->Clear();
        }

        ///
        // Removes the value at the specified index.
        ///
        /*--cef(index_param=index)--*/
        virtual bool Remove(int index)
        {
            return _listValue->Remove(index);
        }

        ///
        // Returns the value type at the specified index.
        ///
        /*--cef(default_retval=VTYPE_INVALID,index_param=index)--*/
        virtual CefSharp::CefValueType GetType(int index)
        {
            return (CefSharp::CefValueType)_listValue->GetType(index);
        }

        ///
        // Returns the value at the specified index as type bool.
        ///
        /*--cef(index_param=index)--*/
        virtual bool GetBool(int index)
        {
            return _listValue->GetBool(index);
        }

        ///
        // Returns the value at the specified index as type int.
        ///
        /*--cef(index_param=index)--*/
        virtual int GetInt(int index)
        {
            return _listValue->GetInt(index);
        }

        ///
        // Returns the value at the specified index as type double.
        ///
        /*--cef(index_param=index)--*/
        virtual double GetDouble(int index)
        {
            return _listValue->GetDouble(index);
        }

        ///
        // Returns the value at the specified index as type string.
        ///
        /*--cef(index_param=index)--*/
        virtual String^ GetString(int index)
        {
            return StringUtils::ToClr(_listValue->GetString(index));
        }

        ///
        // Returns the value at the specified index as type binary.
        ///
        /*--cef(index_param=index)--*/
        virtual array<Byte>^ GetBinary(int index)
        {
            auto binary = _listValue->GetBinary(index);
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
        virtual IDictionaryValue^ GetDictionary(int index)
        {
            throw gcnew NotImplementedException();
            //_listValue->GetDictionary(index);
            //return _listValue->GetDictionary(index);
        }

        ///
        // Returns the value at the specified index as type list.
        ///
        /*--cef(index_param=index)--*/
        virtual IListValue^ GetList(int index)
        {
            return gcnew CefListValueWrapper(_listValue->GetList(index));
        }

        ///
        // Sets the value at the specified index as type null. Returns true if the
        // value was set successfully.
        ///
        /*--cef(index_param=index)--*/
        virtual bool SetNull(int index)
        {
            return _listValue->SetNull(index);
        }

        ///
        // Sets the value at the specified index as type bool. Returns true if the
        // value was set successfully.
        ///
        /*--cef(index_param=index)--*/
        virtual bool SetBool(int index, bool value)
        {
            return _listValue->SetBool(index, value);
        }

        ///
        // Sets the value at the specified index as type int. Returns true if the
        // value was set successfully.
        ///
        /*--cef(index_param=index)--*/
        virtual bool SetInt(int index, int value)
        {
            return _listValue->SetInt(index, value);
        }

        ///
        // Sets the value at the specified index as type double. Returns true if the
        // value was set successfully.
        ///
        /*--cef(index_param=index)--*/
        virtual bool SetDouble(int index, double value)
        {
            return _listValue->SetDouble(index, value);
        }

        ///
        // Sets the value at the specified index as type string. Returns true if the
        // value was set successfully.
        ///
        /*--cef(optional_param=value,index_param=index)--*/
        virtual bool SetString(int index, String^ value)
        {
            return _listValue->SetString(index, StringUtils::ToNative(value));
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
        virtual bool SetBinary(int index, array<Byte>^ value)
        {
            pin_ptr<Byte> src = &value[0];

            auto binary = CefBinaryValue::Create(static_cast<void*>(src), value->Length);
            return _listValue->SetBinary(index, binary);
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
        virtual bool SetDictionary(int index, IDictionaryValue^ value)
        {
            //return _listValue->SetDictionary(index, value);
            return false;
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
        virtual bool SetList(int index, IListValue^ value)
        {
            auto list = (CefListValueWrapper^)value;
            return _listValue->SetList(index, list);
        }
    };
}