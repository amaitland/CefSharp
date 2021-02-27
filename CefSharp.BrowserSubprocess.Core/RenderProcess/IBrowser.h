// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

using namespace System::Collections::Generic;

namespace CefSharp
{
    namespace RenderProcess
    {
        /// <summary>
        /// CefSharp interface for CefBrowser.
        /// </summary>
        public interface class IBrowser : public IDisposable
        {
            /// <summary>
            /// Returns true if the browser can navigate backwards.
            /// </summary>
            property bool CanGoBack
            {
                bool get();
            }

            /// <summary>
            /// Navigate backwards.
            /// </summary>
            void GoBack();

            /// <summary>
            /// Returns true if the browser can navigate forwards.
            /// </summary>
            property bool CanGoForward
            {
                bool get();
            }

            /// <summary>
            /// Navigate forwards.
            /// </summary>
            void GoForward();

            /// <summary>
            /// Returns true if the browser is currently loading.
            /// </summary>
            property bool IsLoading
            {
                bool get();
            }

            /// <summary>
            /// Reload the current page.
            /// </summary>
            /// <param name="ignoreCache">
            /// <c>true</c> a reload is performed ignoring browser cache; <c>false</c> a reload is
            /// performed using files from the browser cache, if available.
            /// </param>
            void Reload(bool ignoreCache);

            /// <summary>
            /// Stop loading the page.
            /// </summary>
            void StopLoad();

            /// <summary>
            /// Returns the globally unique identifier for this browser.
            /// </summary>
            property int Identifier
            {
                int get();
            }

            /// <summary>
            /// Returns true if this object is pointing to the same handle as that object.
            /// </summary>
            /// <param name="that">compare browser instances</param>
            /// <returns>returns true if the same instance</returns>
            bool IsSame(IBrowser^ that);

            /// <summary>
            /// Returns true if the window is a popup window.
            /// </summary>
            property bool IsPopup
            {
                bool get();
            }


            /// <summary>
            /// Returns true if a document has been loaded in the browser.
            /// </summary>
            property bool HasDocument
            {
                bool get();
            }

            /// <summary>
            /// Returns the main (top-level) frame for the browser window.
            /// </summary>
            property IFrame^ MainFrame
            {
                IFrame^ get();
            }

            /// <summary>
            /// Returns the focused frame for the browser window.
            /// </summary>
            property IFrame^ FocusedFrame
            {
                IFrame^ get();
            }

            /// <summary>
            /// Returns the frame with the specified identifier, or NULL if not found.
            /// </summary>
            /// <param name="identifier">identifier</param>
            /// <returns>frame or null</returns>
            IFrame^ GetFrame(Int64 identifier);

            /// <summary>
            /// Returns the frame with the specified name, or NULL if not found.
            /// </summary>
            /// <param name="name">name of frame</param>
            /// <returns>frame or null</returns>
            IFrame^ GetFrame(String^ name);

            /// <summary>
            /// Returns the number of frames that currently exist.
            /// </summary>
            /// <returns>the number of frames</returns>
            int GetFrameCount();

            /// <summary>
            /// Returns the identifiers of all existing frames.
            /// </summary>
            /// <returns>list of frame identifiers</returns>
            List<Int64>^ GetFrameIdentifiers();

            /// <summary>
            /// Returns the names of all existing frames.
            /// </summary>
            /// <returns>frame names</returns>
            List<String^>^ GetFrameNames();

            /// <summary>
            /// Gets a value indicating whether the browser has been disposed of.
            /// </summary>
            property bool IsDisposed
            {
                bool get();
            }
        };
    }
}
