// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_app.h"

using namespace System::Collections::Generic;

namespace CefSharp
{
    // CefApp implementation that's common across all subprocess types
    public class SubProcessApp : public CefApp
    {
    private:
        gcroot<List<Tuple<String^, int>^>^> _schemes;

    public:
        SubProcessApp(List<Tuple<String^, int>^>^ schemes)
        {
            _schemes = schemes;
        }

        ~SubProcessApp()
        {
            delete _schemes;
            _schemes = nullptr;
        }

        void OnRegisterCustomSchemes(CefRawPtr<CefSchemeRegistrar> registrar) OVERRIDE
        {
            for each (auto scheme in _schemes->AsReadOnly())
            {
                auto schemeName = StringUtils::ToNative(scheme->Item1);
                auto schemeOptions = scheme->Item2;
                if (!registrar->AddCustomScheme(schemeName, schemeOptions))
                {
                    LOG(ERROR) << "SubProcessApp::OnRegisterCustomSchemes failed for schemeName:" << schemeName;
                }
            }
        }

        IMPLEMENT_REFCOUNTING(SubProcessApp);
    };
}
