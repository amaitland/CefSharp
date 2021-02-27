// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "V8Exception.h"

using namespace System::Runtime::InteropServices;

namespace CefSharp
{
    namespace RenderProcess
    {
        public interface class IV8Context
        {
            /// <summary>
            /// Execute a string of JavaScript code in this V8 context.
            /// </summary>
            /// <param name="code">JavaScript code to execute</param>
            /// <param name="scriptUrl">Is the URL where the script in question can be found, if any</param>
            /// <param name="startLine">Is the base line number to use for error reporting.</param>
            /// <param name="exception">Is the exception if any.</param>
            /// <returns>On success the function will return true. On failure <paramref name="exception"/> will be set to the exception, if any, and the function will return false.</returns>
            bool Execute(String^ code, String^ scriptUrl, int startLine, [Out] V8Exception^% exception);
        };
    }
}
