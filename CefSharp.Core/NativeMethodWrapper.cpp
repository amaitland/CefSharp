// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "wininet.h"
#include "NativeMethodWrapper.h"

using namespace System;
using namespace System::Runtime::InteropServices;

namespace CefSharp
{
    void NativeMethodWrapper::CopyMemoryUsingHandle(IntPtr dest, IntPtr src, int numberOfBytes)
    {
        CopyMemory(dest.ToPointer(), src.ToPointer(), numberOfBytes);
    }

    bool NativeMethodWrapper::IsFocused(IntPtr handle)
    {
        // Ask Windows which control has the focus and then check if it's one of our children
        auto focusControl = GetFocus();
        return focusControl != 0 && (IsChild((HWND)handle.ToPointer(), focusControl) == 1);
    }

    ProxyInfo NativeMethodWrapper::GetProxyInfo()
    {
        DWORD lpdwBufferLength = 0;
        InternetQueryOption(NULL, INTERNET_OPTION_PROXY, NULL, &lpdwBufferLength);
        auto buffer = Marshal::AllocHGlobal(lpdwBufferLength);

        try
        {
            if (InternetQueryOption(NULL, INTERNET_OPTION_PROXY, (void*)buffer, &lpdwBufferLength))
            {
                return (ProxyInfo)Marshal::PtrToStructure(buffer, ProxyInfo::typeid);
            }
        }
        finally
        {
            if (buffer != IntPtr::Zero)
            {
                Marshal::FreeHGlobal(buffer);
            }
        }
    }
}
