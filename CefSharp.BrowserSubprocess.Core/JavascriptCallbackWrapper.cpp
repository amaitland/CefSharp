// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "stdafx.h"
#include "TypeUtils.h"
#include "JavascriptCallbackWrapper.h"
#include "JavascriptCallbackRegistry.h"
#include "Serialization/V8Serialization.h"

#include ".\..\CefSharp.Core\Internals\CefProcessMessageWrapper.h"
#include "..\CefSharp.Core\Internals\Serialization\Primitives.h"

using namespace CefSharp::Internals::Serialization;

namespace CefSharp
{
    namespace Internals
    {
        IProcessMessage^ JavascriptCallbackWrapper::Execute(IProcessMessage^ request, JavascriptCallbackRegistry^ callbackRegistry)
        {
            auto messageWrapper = (CefProcessMessageWrapper^)request;
            auto message = (CefRefPtr<CefProcessMessage>)messageWrapper;
            auto argList = message->GetArgumentList();
            
            auto jsCallbackId = GetInt64(argList, 0);
            auto callbackId = GetInt64(argList, 1);
            auto parameterList = argList->GetList(2);
            auto success = false;
            CefString errorMessage;

            CefV8ValueList params;
            for (CefV8ValueList::size_type i = 0; i < parameterList->GetSize(); i++)
            {
                params.push_back(DeserializeV8Object(parameterList, static_cast<int>(i)));
            }

            auto response = CefProcessMessage::Create(StringUtils::ToNative(Messages::JavascriptCallbackResponse));

            if (_context.get() && _context->Enter())
            {
                try
                {
                    auto result = _value->ExecuteFunction(nullptr, params);
                    success = result.get() != nullptr;
                        
                    //we need to do this here to be able to store the v8context
                    if (success)
                    {
                        auto responseArgList = response->GetArgumentList();
                        SerializeV8Object(result, responseArgList, 2, callbackRegistry);
                    }
                    else
                    {
                        auto exception = _value->GetException();
                        if(exception.get())
                        {
                            errorMessage = exception->GetMessage();
                        }
                    }
                }
                finally
                {
                    _context->Exit();
                }
            }
            else
            {
                errorMessage = "Unable to Enter Context";			
            }

            if (response.get())
            {
                auto responseArgList = response->GetArgumentList();
                responseArgList->SetBool(0, success);
                SetInt64(callbackId, responseArgList, 1);
                if (!success)
                {
                    responseArgList->SetString(2, errorMessage);
                }
            }

            return gcnew CefProcessMessageWrapper(response);
        }
    }
}
