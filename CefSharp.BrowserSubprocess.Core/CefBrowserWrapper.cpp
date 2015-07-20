// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
#pragma once

#include "Stdafx.h"

#include "CefBrowserWrapper.h"
#include "Serialization/V8Serialization.h"
#include ".\..\CefSharp.Core\Internals\CefProcessMessageWrapper.h"
#include ".\..\CefSharp.Core\Internals\Serialization\Primitives.h"

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

    bool CefBrowserWrapper::SendProcessMessage(ProcessId targetProcess, IProcessMessage^ message)
    {
        auto messageWrapper = (CefProcessMessageWrapper^)message;

        return _browser->SendProcessMessage((CefProcessId)targetProcess, (CefRefPtr<CefProcessMessage>)messageWrapper);
    }

    IProcessMessage^ CefBrowserWrapper::EvalScriptInFrame(IProcessMessage^ request)
    {
        auto messageWrapper = (CefProcessMessageWrapper^)request;
        auto message = (CefRefPtr<CefProcessMessage>)messageWrapper;
        auto argList = message->GetArgumentList();

        //auto frameId = GetInt64(argList, 0);
        //auto script = argList->GetString(2);
        //auto success = false;

        //auto response = CefProcessMessage::Create(StringUtils::ToNative(Messages::EvaluateJavascriptResponse));

        //auto frame = _browser->GetFrame(frameId);
        //if (frame.get())
        //{
        //    auto context = frame->GetV8Context();
        //            
        //    if (context.get() && context->Enter())
        //    {
        //        try
        //        {
        //            CefRefPtr<CefV8Exception> exception;
        //            success = context->Eval(script, result, exception);
        //                    
        //            //we need to do this here to be able to store the v8context
        //            if (success)
        //            {
        //                auto responseArgList = response->GetArgumentList();
        //                SerializeV8Object(result, responseArgList, 2, CallbackRegistry);
        //            }
        //            else
        //            {
        //                errorMessage = exception->GetMessage();
        //            }
        //        }
        //        finally
        //        {
        //            context->Exit();
        //        }
        //    }
        //    else
        //    {
        //        errorMessage = "Unable to Enter Context";
        //    }
        //}
        //else
        //{
        //    errorMessage = "Unable to Get Frame matching Id";
        //}

        //if (response.get())
        //{
        //    auto responseArgList = response->GetArgumentList();
        //    responseArgList->SetBool(0, success);
        //    SetInt64(callbackId, responseArgList, 1);
        //    if (!success)
        //    {
        //        responseArgList->SetString(2, errorMessage);
        //    }
        //    browser->SendProcessMessage(sourceProcessId, response);
        //}
        return nullptr;
    }
}