// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "CefBrowserWrapper.h"
#include ".\..\CefSharp.Core\Internals\CefProcessMessageWrapper.h"

namespace CefSharp
{
    JavascriptRootObjectWrapper^ CefBrowserWrapper::JavascriptRootObjectWrapper::get()
    {
        return _javascriptRootObjectWrapper;
    }

    void CefBrowserWrapper::JavascriptRootObjectWrapper::set(CefSharp::JavascriptRootObjectWrapper^ value)
    {
        _javascriptRootObjectWrapper = value;
        if (_javascriptRootObjectWrapper != nullptr)
        {
            _javascriptRootObjectWrapper->CallbackRegistry = _callbackRegistry;
        }
    }

    JavascriptCallbackRegistry^ CefBrowserWrapper::CallbackRegistry::get()
    {
        return _callbackRegistry;
    }

    bool CefBrowserWrapper::SendProcessMessage(IProcessMessage^ message)
    {
        auto messageWrapper = (CefProcessMessageWrapper^)message;

        return _browser->SendProcessMessage(CefProcessId::PID_BROWSER, (CefRefPtr<CefProcessMessage>)messageWrapper);
    }

    CefFrameWrapper^ CefBrowserWrapper::GetFrame(int64 frameId)
    {
        auto frame = _browser->GetFrame(frameId);

        if(frame.get())
        {
            return gcnew CefFrameWrapper(frame);
        }
        return nullptr;
    }
}