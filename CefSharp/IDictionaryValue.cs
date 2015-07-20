// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.using System;

using System;
using System.Collections.Generic;

namespace CefSharp
{
    public interface IDictionaryValue : IDisposable
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
        // Returns a writable copy of this object. If |exclude_empty_children| is true
        // any empty dictionaries or lists will be excluded from the copy.
        ///
        /*--cef()--*/
        IDictionaryValue Copy(bool excludeEmptyChildren);

        ///
        // Returns the number of values.
        ///
        /*--cef()--*/
        UIntPtr Size { get; }

        ///
        // Removes all values. Returns true on success.
        ///
        /*--cef()--*/
        bool Clear();

        ///
        // Returns true if the current dictionary has a value for the given key.
        ///
        /*--cef()--*/
        bool HasKey(string key);

        ///
        // Reads all keys for this dictionary into the specified vector.
        ///
        /*--cef()--*/
        bool GetKeys(out IList<string> keys);

        ///
        // Removes the value at the specified key. Returns true is the value was
        // removed successfully.
        ///
        /*--cef()--*/
        bool Remove(string key);

        ///
        // Returns the value type for the specified key.
        ///
        /*--cef(default_retval=VTYPE_INVALID)--*/
        CefValueType GetType(string key);

        ///
        // Returns the value at the specified key as type bool.
        ///
        /*--cef()--*/
        bool GetBool(string key);

        ///
        // Returns the value at the specified key as type int.
        ///
        /*--cef()--*/
        int GetInt(string key);

        ///
        // Returns the value at the specified key as type double.
        ///
        /*--cef()--*/
        double GetDouble(string key);

        ///
        // Returns the value at the specified key as type string.
        ///
        /*--cef()--*/
        string GetString(string key);

        ///
        // Returns the value at the specified key as type binary.
        ///
        /*--cef()--*/
        byte[] GetBinary(string key);

        ///
        // Returns the value at the specified key as type dictionary.
        ///
        /*--cef()--*/
        IDictionaryValue GetDictionary(string key);

        ///
        // Returns the value at the specified key as type list.
        ///
        /*--cef()--*/
        IListValue GetList(string key);

        ///
        // Sets the value at the specified key as type null. Returns true if the
        // value was set successfully.
        ///
        /*--cef()--*/
        bool SetNull(string key);

        ///
        // Sets the value at the specified key as type bool. Returns true if the
        // value was set successfully.
        ///
        /*--cef()--*/
        bool SetBool(string key, bool value);

        ///
        // Sets the value at the specified key as type int. Returns true if the
        // value was set successfully.
        ///
        /*--cef()--*/
        bool SetInt(string key, int value);

        ///
        // Sets the value at the specified key as type double. Returns true if the
        // value was set successfully.
        ///
        /*--cef()--*/
        bool SetDouble(string key, double value);

        ///
        // Sets the value at the specified key as type string. Returns true if the
        // value was set successfully.
        ///
        /*--cef(optional_param=value)--*/
        bool SetString(string key, string value);

        ///
        // Sets the value at the specified key as type binary. Returns true if the
        // value was set successfully. If |value| is currently owned by another object
        // then the value will be copied and the |value| reference will not change.
        // Otherwise, ownership will be transferred to this object and the |value|
        // reference will be invalidated.
        ///
        /*--cef()--*/
        bool SetBinary(string key, byte[] value);

        ///
        // Sets the value at the specified key as type dict. Returns true if the
        // value was set successfully. After calling this method the |value| object
        // will no longer be valid. If |value| is currently owned by another object
        // then the value will be copied and the |value| reference will not change.
        // Otherwise, ownership will be transferred to this object and the |value|
        // reference will be invalidated.
        ///
        /*--cef()--*/
        bool SetDictionary(string key, IDictionaryValue value);

        ///
        // Sets the value at the specified key as type list. Returns true if the
        // value was set successfully. After calling this method the |value| object
        // will no longer be valid. If |value| is currently owned by another object
        // then the value will be copied and the |value| reference will not change.
        // Otherwise, ownership will be transferred to this object and the |value|
        // reference will be invalidated.
        ///
        /*--cef()--*/
        bool SetList(string key, IListValue value);
    }
}
