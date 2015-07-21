// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_browser.h"
#include "include/cef_runnable.h"
#include "include/cef_v8.h"

#include "TypeUtils.h"
#include "Stdafx.h"
#include "JavascriptRootObjectWrapper.h"
#include "CefFrameWrapper.h"

using namespace CefSharp::Internals;
using namespace System;
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

    public:
        CefBrowserWrapper(CefRefPtr<CefBrowser> browser)
        {
            _browser = browser;
            BrowserId = browser->GetIdentifier();
        }
        
        !CefBrowserWrapper()
        {
            _browser = nullptr;
        }

        ~CefBrowserWrapper()
        {
            this->!CefBrowserWrapper();
        }

        operator CefRefPtr<CefBrowser>()
        {
            return _browser.get();
        }

        property int BrowserId;

        // The serialized registered object data waiting to be used.
        property JavascriptRootObject^ JavascriptRootObject;

        property JavascriptRootObjectWrapper^ JavascriptRootObjectWrapper;

        // The WCF proxy to the parent process.
        property IBrowserProcess^ BrowserProcess;

        bool SendProcessMessage(IProcessMessage^ message);

        CefFrameWrapper^ GetFrame(int64 frameId);
    };
}
