// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "CefAppWrapper.h"
#include ".\..\CefSharp.Core\Internals\CefProcessMessageWrapper.h"

using namespace System;
using namespace System::Diagnostics;
using namespace System::Collections::Generic;

namespace CefSharp
{
    int CefAppWrapper::Run()
    {
        auto hInstance = Process::GetCurrentProcess()->Handle;

        CefMainArgs cefMainArgs((HINSTANCE)hInstance.ToPointer());

        return CefExecuteProcess(cefMainArgs, (CefApp*)_cefApp.get(), NULL);
    }

    IProcessMessage^ CefAppWrapper::CreateProcessMessage(String^ name)
    {
        return gcnew CefProcessMessageWrapper(CefProcessMessage::Create(StringUtils::ToNative(name)));
    }
}