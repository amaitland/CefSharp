// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

using namespace System::Collections::Generic;

namespace CefSharp
{
    namespace BrowserSubprocess
    {
        private ref class CommandLineArgsParser
        {
        internal:
            static bool HasArgument(IEnumerable<String^>^ args, String^ argumentName)
            {
                for each (auto arg in args)
                {
                    if (arg->StartsWith(argumentName))
                    {
                        return true;
                    }
                }

                return false;
            }

            static String^ GetArgumentValue(IEnumerable<String^>^ args, String^ argumentName)
            {
                String^ match = nullptr;;

                for each (auto arg in args)
                {
                    //Found a match
                    //Technically args can be duplicated and Chromium takes the last value,
                    //For the args we're using this shouldn't be a problem as we've used this
                    //logic for a long long time, if we need to remove the break and take the last match
                    if (arg->StartsWith(argumentName))
                    {
                        match = arg;
                        break;
                    }
                }

                //No matching argument, return nullptr
                if (match == nullptr)
                {
                    return nullptr;
                }

                auto split = match->Split('=');
                if (split->Length < 2)
                {
                    return nullptr;
                }

                //found a match, return it's value
                return split[1];
            }
        };
    }
}

