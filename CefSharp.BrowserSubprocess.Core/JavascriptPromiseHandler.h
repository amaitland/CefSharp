// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include/cef_v8.h"
#include "Serialization\V8Serialization.h"

namespace CefSharp
{
	private class JavascriptPromiseHandler : public CefV8Handler
	{
	public:
		JavascriptPromiseHandler()
		{
		}

		~JavascriptPromiseHandler()
		{
		}

		virtual bool Execute(const CefString& name,
			CefRefPtr<CefV8Value> object,
			const CefV8ValueList& arguments,
			CefRefPtr<CefV8Value>& retval,
			CefString& exception)
		{
			if (name == "PromiseThenFunction" && arguments[0]->IsInt())
			{
				auto callbackId = arguments[0]->GetIntValue();

				auto response = CefProcessMessage::Create(kEvaluateJavascriptResponse);

				auto responseArgList = response->GetArgumentList();
				responseArgList->SetBool(0, true);
				SetInt64(responseArgList, 1, callbackId);
				if (exception != "")
				{
					responseArgList->SetString(2, exception);
				}
				else
				{
					SerializeV8Object(arguments[1], responseArgList, 2, nullptr);
				}

				auto context = CefV8Context::GetCurrentContext();

				auto browser = context->GetBrowser();

				browser->SendProcessMessage(CefProcessId::PID_BROWSER, response);
			}
			return false;
		}

		IMPLEMENT_REFCOUNTING(JavascriptPromiseHandler)
	};
}
