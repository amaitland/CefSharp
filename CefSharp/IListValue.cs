// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.using System;

using System;
using System.Collections.Generic;

namespace CefSharp
{
    public interface IListValue : IDisposable
    {
        ///
        // Returns true if this object is valid. Do not call any other methods if this
        // method returns false.
        ///
        /*--cef()--*/
        bool IsValid { get; }

        ///
        // Returns true if this object is currently owned by another object.
        ///
        /*--cef()--*/
        bool IsOwned { get; }

        ///
        // Returns true if the values of this object are read-only. Some APIs may
        // expose read-only objects.
        ///
        /*--cef()--*/
        bool IsReadOnly { get; }

        ///
        // Returns a writable copy of this object.
        ///
        /*--cef()--*/
        IListValue Copy();

        ///
        // Sets the number of values. If the number of values is expanded all
        // new value slots will default to type null. Returns true on success.
        ///
        /*--cef()--*/
        bool SetSize(UIntPtr size);

        ///
        // Returns the number of values.
        ///
        /*--cef()--*/
        UIntPtr GetSize();

        ///
        // Removes all values. Returns true on success.
        ///
        /*--cef()--*/
        bool Clear();

        ///
        // Removes the value at the specified index.
        ///
        /*--cef(index_param=index)--*/
        bool Remove(int index);

        ///
        // Returns the value type at the specified index.
        ///
        /*--cef(default_retval=VTYPE_INVALID,index_param=index)--*/
        CefValueType GetType(int index);

        ///
        // Returns the value at the specified index as type bool.
        ///
        /*--cef(index_param=index)--*/
        bool GetBool(int index);

        ///
        // Returns the value at the specified index as type int.
        ///
        /*--cef(index_param=index)--*/
        int GetInt(int index);

        ///
        // Returns the value at the specified index as type double.
        ///
        /*--cef(index_param=index)--*/
        double GetDouble(int index);

        ///
        // Returns the value at the specified index as type string.
        ///
        /*--cef(index_param=index)--*/
        string GetString(int index);

        ///
        // Returns the value at the specified index as type binary.
        ///
        /*--cef(index_param=index)--*/
        byte[] GetBinary(int index);

        ///
        // Returns the value at the specified index as type dictionary.
        ///
        /*--cef(index_param=index)--*/
        IDictionaryValue GetDictionary(int index);

        ///
        // Returns the value at the specified index as type list.
        ///
        /*--cef(index_param=index)--*/
        IListValue GetList(int index);

        ///
        // Sets the value at the specified index as type null. Returns true if the
        // value was set successfully.
        ///
        /*--cef(index_param=index)--*/
        bool SetNull(int index);

        ///
        // Sets the value at the specified index as type bool. Returns true if the
        // value was set successfully.
        ///
        /*--cef(index_param=index)--*/
        bool SetBool(int index, bool value);

        ///
        // Sets the value at the specified index as type int. Returns true if the
        // value was set successfully.
        ///
        /*--cef(index_param=index)--*/
        bool SetInt(int index, int value);

        ///
        // Sets the value at the specified index as type double. Returns true if the
        // value was set successfully.
        ///
        /*--cef(index_param=index)--*/
        bool SetDouble(int index, double value);

        ///
        // Sets the value at the specified index as type string. Returns true if the
        // value was set successfully.
        ///
        /*--cef(optional_param=value,index_param=index)--*/
        bool SetString(int index, string value);

        ///
        // Sets the value at the specified index as type binary. Returns true if the
        // value was set successfully. After calling this method the |value| object
        // will no longer be valid. If |value| is currently owned by another object
        // then the value will be copied and the |value| reference will not change.
        // Otherwise, ownership will be transferred to this object and the |value|
        // reference will be invalidated.
        ///
        /*--cef(index_param=index)--*/
        bool SetBinary(int index, byte[] value);

        ///
        // Sets the value at the specified index as type dict. Returns true if the
        // value was set successfully. After calling this method the |value| object
        // will no longer be valid. If |value| is currently owned by another object
        // then the value will be copied and the |value| reference will not change.
        // Otherwise, ownership will be transferred to this object and the |value|
        // reference will be invalidated.
        ///
        /*--cef(index_param=index)--*/
        bool SetDictionary(int index, IDictionaryValue value);

        ///
        // Sets the value at the specified index as type list. Returns true if the
        // value was set successfully. After calling this method the |value| object
        // will no longer be valid. If |value| is currently owned by another object
        // then the value will be copied and the |value| reference will not change.
        // Otherwise, ownership will be transferred to this object and the |value|
        // reference will be invalidated.
        ///
        /*--cef(index_param=index)--*/
        bool SetList(int index, IList<IListValue> value);
    }
}
