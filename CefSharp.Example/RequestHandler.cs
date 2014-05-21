// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Net;

namespace CefSharp.Example
{
	public class RequestHandler<TAuthDialog> : IRequestHandler where TAuthDialog : IAuthDialog, new()
	{
		public bool OnBeforeBrowse(IWebBrowser browser, IRequest request, bool isRedirect)
		{
			return false;
		}

		public bool OnBeforeResourceLoad(IWebBrowser browser, IRequestResponse requestResponse)
		{
			return false;
		}

		public void OnResourceResponse(IWebBrowser browser, string url, int status, string statusText, string mimeType, WebHeaderCollection headers)
		{
			
		}

		public bool GetDownloadHandler(IWebBrowser browser, out IDownloadHandler handler)
		{
			handler = new DownloadHandler();
			return true;
		}

		public bool GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme, ref string username, ref string password)
		{
			var authDialog = new TAuthDialog();
			if (authDialog.ShowDialog() == true)
			{
				username = authDialog.UserName;
				password = authDialog.Password;

				return true;
			}

			return false;
		}
	}
}
