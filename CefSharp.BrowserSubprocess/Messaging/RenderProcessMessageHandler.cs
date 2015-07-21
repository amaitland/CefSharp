// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CefSharp.Internals;

namespace CefSharp.BrowserSubprocess.Messaging
{
	public class RenderProcessMessageHandler : IBrowserProcess
	{
		private readonly WeakReference browserWeakReference;

		public PendingTaskRepository<BrowserProcessResponse> TaskRepository { get; private set; }

		public RenderProcessMessageHandler(CefBrowserWrapper browser)
		{
			TaskRepository = new PendingTaskRepository<BrowserProcessResponse>();
			browserWeakReference = new WeakReference(browser);
		}

		public Task<BrowserProcessResponse> CallMethodAsync(long objectId, string name, IList<CefV8ValueWrapper> parameters)
		{
			var browser = GetBrowser();

			if (browser != null)
			{
				var idAndComplectionSource = TaskRepository.CreatePendingTask();

				var message = browser.CreateProcessMessage(Messages.CallMethodRequest);

				var i = 0;
				var argList = message.ArgumentList;
				argList.SetInt64(i++, i++, idAndComplectionSource.Key);
				argList.SetInt64(i++, i++, objectId);
				argList.SetString(i++, name);
				argList.SetInt(i++, parameters.Count);
				foreach(var param in parameters)
				{
					//TODO: Populate callback repoisitory and v8Context
					param.SerializeV8Object(argList, i++, null, null);
				}

				browser.SendProcessMessage(message);

				return idAndComplectionSource.Value.Task;
			}
			return null;
		}

		public Task<BrowserProcessResponse> GetPropertyAsync(long objectId, string name)
		{
			return null;
		}

		public Task<BrowserProcessResponse> SetPropertyAsync(long objectId, string name, object value)
		{
			return null;
		}

		private CefBrowserWrapper GetBrowser()
		{
			if (browserWeakReference.IsAlive)
			{
				return browserWeakReference.Target as CefBrowserWrapper;
			}

			return null;
		}
	}
}
