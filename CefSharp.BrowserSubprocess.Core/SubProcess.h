// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_app.h"

#include "SubProcessApp.h"
#include "CefBrowserWrapper.h"
#include "CefAppUnmanagedWrapper.h"
#include "Cef.h"
#include "CommandLineArgsParser.h"

using namespace System::Collections::Generic;
using namespace System::Linq;
using namespace CefSharp::RenderProcess;

namespace CefSharp
{
    namespace BrowserSubprocess
    {
        // Wrap CefAppUnmangedWrapper in a nice managed wrapper
        public ref class SubProcess
        {
        private:
            MCefRefPtr<CefAppUnmanagedWrapper> _cefApp;

        public:
            SubProcess(IRenderProcessHandler^ handler, IEnumerable<String^>^ args)
            {
                auto onBrowserCreated = gcnew Action<CefBrowserWrapper^>(this, &SubProcess::OnBrowserCreated);
                auto onBrowserDestroyed = gcnew Action<CefBrowserWrapper^>(this, &SubProcess::OnBrowserDestroyed);
                auto schemes = ParseCommandLineArguments(args);
                auto enableFocusedNodeChanged = CommandLineArgsParser::HasArgument(args, CefSharpArguments::FocusedNodeChangedEnabledArgument);

                _cefApp = new CefAppUnmanagedWrapper(handler, schemes, enableFocusedNodeChanged, onBrowserCreated, onBrowserDestroyed);
            }

            !SubProcess()
            {
                _cefApp = nullptr;
            }

            ~SubProcess()
            {
                this->!SubProcess();
            }

            int Run()
            {
                auto hInstance = Process::GetCurrentProcess()->Handle;

                CefMainArgs cefMainArgs((HINSTANCE)hInstance.ToPointer());

                return CefExecuteProcess(cefMainArgs, (CefApp*)_cefApp.get(), NULL);
            }

            virtual void OnBrowserCreated(CefBrowserWrapper^ cefBrowserWrapper)
            {

            }

            virtual void OnBrowserDestroyed(CefBrowserWrapper^ cefBrowserWrapper)
            {

            }

            static void EnableHighDPISupport()
            {
                CefEnableHighDPISupport();
            }

            static int ExecuteProcess(IEnumerable<String^>^ args)
            {
                auto hInstance = Process::GetCurrentProcess()->Handle;

                CefMainArgs cefMainArgs((HINSTANCE)hInstance.ToPointer());

                auto schemes = ParseCommandLineArguments(args);

                CefRefPtr<CefApp> app = new SubProcessApp(schemes);

                return CefExecuteProcess(cefMainArgs, app, NULL);
            }

            /// <summary>
            /// Method used internally
            /// </summary>
            /// <param name="args">command line arguments</param>
            /// <returns>list of scheme objects</returns>
            static List<Tuple<String^, int>^>^ ParseCommandLineArguments(IEnumerable<String^>^ args)
            {
                //Needs to be kept in sync with CefSharp::Internals::CefSharpArguments::CustomSchemeArgument
                //Shortly we'll remove the reference on CefSharp.dll for the NetCore projects
                auto schemes = CommandLineArgsParser::GetArgumentValue(args, "--custom-scheme");
                auto customSchemes = gcnew List<Tuple<String^, int>^>();

                if (!String::IsNullOrEmpty(schemes))
                {
                    auto schemeTokens = schemes->Split(';');
                    for each (auto x in schemeTokens)
                    {
                        auto tokens = x->Split('|');
                        auto schemeName = tokens[0];
                        int schemeOptions = 0;

                        if (int::TryParse(tokens[1], schemeOptions))
                        {
                            auto customScheme = gcnew Tuple<String^, int>(schemeName, schemeOptions);

                            customSchemes->Add(customScheme);
                        }
                        else
                        {
                            LOG(ERROR) << "SubProcess::ParseCommandLineArguments failed for schemeName:" << StringUtils::ToNative(schemeName);
                        }
                    }
                }

                return customSchemes;
            }
        };
    }
}
