// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Internals
{
	public static class Messages
	{
		//Message containing a script to be evaluated
		public const string EvaluateJavascriptRequest = "EvaluateJavascriptRequest";
		//Message containing the result for a given evaluation
		public const string EvaluateJavascriptResponse = "EvaluateJavascriptDoneResponse";
		//Message to invoke a stored js function
		public const string JavascriptCallbackRequest = "JavascriptCallbackRequest";
		//Message to dereference a stored js function
		public const string JavascriptCallbackDestroyRequest = "JavascriptCallbackDestroyRequest";
		//Message containing the result of a given js function call
		public const string JavascriptCallbackResponse = "JavascriptCallbackDoneResponse";

		public const string RegisterJavascriptObjectsRequest = "RegisterJavascriptObjectsRequest";

		public const string CallMethodRequest = "CallMethodRequest";
		public const string CallMethodResponse = "CallMethodResponse";
	}
}
