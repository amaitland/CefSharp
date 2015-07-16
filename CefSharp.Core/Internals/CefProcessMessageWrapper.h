// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "CefListValueWrapper.h"

namespace CefSharp
{
    public ref class CefProcessMessageWrapper : public IProcessMessage
    {
    private:
        MCefRefPtr<CefProcessMessage> _processMessage;
        CefListValueWrapper^ _listValue;

    public:
        CefProcessMessageWrapper(CefRefPtr<CefProcessMessage> &processMessage) : _processMessage(processMessage)
        {
            
        }

        !CefProcessMessageWrapper()
        {
            _processMessage = NULL;
        }

        ~CefProcessMessageWrapper()
        {
            delete _listValue;

            this->!CefProcessMessageWrapper();
        }

        ///
        // Returns the Process Message Name
        ///
        /*--cef()--*/
        virtual property String^ Name
        {
            String^ get()
            {
                return StringUtils::ToClr(_processMessage->GetName());
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
                return _processMessage->IsReadOnly();
            }
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
                return _processMessage->IsValid();
            }
        }

        virtual property IListValue^ ArgumentList
        {
            IListValue^ get()
            {
                if(_listValue == nullptr)
                {
                    _listValue = gcnew CefListValueWrapper(_processMessage->GetArgumentList());
                }                

                return _listValue;
            }
        }

        MCefRefPtr<CefProcessMessage> GetProcessMessage()
        {
            return _processMessage;
        }
    };
}