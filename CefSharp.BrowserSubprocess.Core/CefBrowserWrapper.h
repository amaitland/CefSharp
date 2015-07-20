﻿// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_browser.h"
#include "include/cef_runnable.h"
#include "include/cef_v8.h"

#include "TypeUtils.h"
#include "Stdafx.h"
#include "JavascriptRootObjectWrapper.h"

using namespace CefSharp::Internals;
using namespace System;
using namespace System::ServiceModel;
using namespace System::Threading;
using namespace System::Threading::Tasks;

namespace CefSharp
{
    // "Master class" for wrapping everything that the Cef Subprocess needs 
    // for ONE CefBrowser.
    public ref class CefBrowserWrapper
    {
    private:
        MCefRefPtr<CefBrowser> _browser;
        JavascriptCallbackRegistry^ _callbackRegistry;
        JavascriptRootObjectWrapper^ _javascriptRootObjectWrapper;

    internal:
        property JavascriptCallbackRegistry^ CallbackRegistry
        {
            CefSharp::Internals::JavascriptCallbackRegistry^ get();
        }

    public:
        CefBrowserWrapper(CefRefPtr<CefBrowser> browser)
        {
            _browser = browser;
            BrowserId = browser->GetIdentifier();
            IsPopup = browser->IsPopup();
            _callbackRegistry = gcnew JavascriptCallbackRegistry(BrowserId);
        }
        
        !CefBrowserWrapper()
        {
            _browser = nullptr;
        }

        ~CefBrowserWrapper()
        {
            this->!CefBrowserWrapper();
            if (_callbackRegistry != nullptr)
            {
                delete _callbackRegistry;
                _callbackRegistry = nullptr;
            }
        }

        property int BrowserId;
        property bool IsPopup;

        // This allows us to create the WCF proxies back to our parent process.
        property ChannelFactory<IBrowserProcess^>^ ChannelFactory;

        // The serialized registered object data waiting to be used.
        property JavascriptRootObject^ JavascriptRootObject;

        property JavascriptRootObjectWrapper^ JavascriptRootObjectWrapper 
        {
            CefSharp::JavascriptRootObjectWrapper^ get();
            void set(CefSharp::JavascriptRootObjectWrapper^ value);
        };

        // The WCF proxy to the parent process.
        property IBrowserProcess^ BrowserProcess;

        bool SendProcessMessage(ProcessId targetProcess, IProcessMessage^ message);
    };
}
