// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "CefProcessMessageWrapper.h"

namespace CefSharp
{
    public ref class ManagedWrapperFactory : public IManagedWrapperFactory
    {
    public:
        virtual IProcessMessage^ CreateProcessMessage(String^ name)
        {
            return gcnew CefProcessMessageWrapper(CefProcessMessage::Create(StringUtils::ToNative(name)));
        }
    };
}