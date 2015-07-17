// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Internals
{
	public static class ManagedWrapperFactory
	{
		public static Func<string, IProcessMessage> CreateProcessMessage { get; set; }
	}
}
