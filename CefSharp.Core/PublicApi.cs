// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;

//TODO: Split out into seperate classes
namespace CefSharp
{
    /// <inheritdoc/>
    public class BrowserSettings : CefSharp.Core.BrowserSettings
    {
        /// <inheritdoc/>
        public BrowserSettings(bool autoDispose = false) : base(autoDispose)
        {

        }
    }

    /// <inheritdoc/>
    public sealed class Cef : CefSharp.Core.Cef
    {
        
    }

    /// <inheritdoc/>
    public abstract class CefSettingsBase : CefSharp.Core.CefSettingsBase
    {
        
    }

    /// <inheritdoc/>
    public partial class ManagedCefBrowserAdapter : CefSharp.Core.ManagedCefBrowserAdapter
    {
        public ManagedCefBrowserAdapter(CefSharp.Internals.IWebBrowserInternal webBrowserInternal, bool offScreenRendering) : base(webBrowserInternal, offScreenRendering)
        {
        }
    }

    /// <inheritdoc/>
    public sealed class NativeMethodWrapper : CefSharp.Core.NativeMethodWrapper
    {
        
    }

    /// <inheritdoc/>
    public class PostData : CefSharp.Core.PostData
    {
        
    }

    /// <inheritdoc/>
    public class PostDataElement : CefSharp.Core.PostDataElement
    {
        
    }

    /// <inheritdoc/>
    public class Request : CefSharp.Core.Request
    {
        
    }

    /// <inheritdoc/>
    public class RequestContext : CefSharp.Core.RequestContext
    {
        /// <inheritdoc/>
        public RequestContext() : base()
        {
        }

        /// <inheritdoc/>
        public RequestContext(CefSharp.IRequestContext otherRequestContext) : base (otherRequestContext)
        {

        }

        /// <inheritdoc/>
        public RequestContext(CefSharp.IRequestContext otherRequestContext, CefSharp.IRequestContextHandler requestContextHandler) : base(otherRequestContext, requestContextHandler)
        {
        }

        /// <inheritdoc/>
        public RequestContext(CefSharp.IRequestContextHandler requestContextHandler) : base(requestContextHandler)
        {
        }

        /// <inheritdoc/>
        public RequestContext(CefSharp.RequestContextSettings settings) : base(settings)
        {

        }

        /// <inheritdoc/>
        public RequestContext(CefSharp.RequestContextSettings settings, CefSharp.IRequestContextHandler requestContextHandler): base (settings, requestContextHandler)
        {
        }

        /// <summary>
        /// Creates a new RequestContextBuilder which can be used to fluently set
        /// preferences
        /// </summary>
        /// <returns>Returns a new RequestContextBuilder</returns>
        public static RequestContextBuilder Configure()
        {
            return new RequestContextBuilder();
        }
    }

    /// <inheritdoc/>
    public class RequestContextBuilder : CefSharp.Core.RequestContextBuilder
    {
        
    }

    /// <inheritdoc/>
    public class RequestContextSettings : CefSharp.Core.RequestContextSettings
    {
        
    }

    /// <inheritdoc/>
    public class UrlRequest : CefSharp.Core.UrlRequest
    {
        public UrlRequest(IRequest request, IUrlRequestClient urlRequestClient) : base(request, urlRequestClient)
        {
        }

        public UrlRequest(IRequest request, IUrlRequestClient urlRequestClient, IRequestContext requestContext) : base(request, urlRequestClient, requestContext)
        {
        }
    }

    /// <inheritdoc/>
    public class WindowInfo : CefSharp.Core.WindowInfo
    {
        
    }

    public static class DragData
    {
        public static IDragData Create()
        {
            return CefSharp.Internals.CefDragDataWrapper.Create();
        }
    }
}
