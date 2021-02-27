// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

namespace CefSharp
{
    namespace RenderProcess
    {
        /// <summary>
        /// This interface represents a CefFrame object (i.e. a HTML frame)
        /// </summary>
        public interface class IFrame : public IDisposable
        {
            /// <summary>
            /// True if this object is currently attached to a valid frame.
            /// </summary>
            property bool IsValid
            {
                bool get();
            }

            /// <summary>
            /// Execute undo in this frame.
            /// </summary>
            void Undo();

            /// <summary>
            /// Execute redo in this frame.
            /// </summary>
            void Redo();

            /// <summary>
            /// Execute cut in this frame.
            /// </summary>
            void Cut();

            /// <summary>
            /// Execute copy in this frame.
            /// </summary>
            void Copy();

            /// <summary>
            /// Execute paste in this frame.
            /// </summary>
            void Paste();

            /// <summary>
            /// Execute delete in this frame.
            /// </summary>
            void Delete();

            /// <summary>
            /// Execute select all in this frame.
            /// </summary>
            void SelectAll();

            /// <summary>
            /// Load the specified url.
            /// </summary>
            /// <param name="url">url to be loaded in the frame</param>
            void LoadUrl(String^ url);

            /// <summary>
            /// Execute a string of JavaScript code in this frame.
            /// </summary>
            /// <param name="code">Javascript to execute</param>
            /// <param name="scriptUrl">is the URL where the script in question can be found, if any.
            /// The renderer may request this URL to show the developer the source of the error.</param>
            /// <param name="startLine">is the base line number to use for error reporting.</param>
            void ExecuteJavaScriptAsync(String^ code, String^ scriptUrl, int startLine);

            /// <summary>
            /// Returns true if this is the main (top-level) frame.
            /// </summary>
            property bool IsMain
            {
                bool get();
            }

            /// <summary>
            /// Returns true if this is the focused frame.
            /// </summary>
            property bool IsFocused
            {
                bool get();
            }

            /// <summary>
            /// Returns the name for this frame. If the frame has an assigned name (for
            /// example, set via the iframe "name" attribute) then that value will be
            /// returned. Otherwise a unique name will be constructed based on the frame
            /// parent hierarchy. The main (top-level) frame will always have an empty name
            /// value.
            /// </summary>
            property String^ Name
            {
                String^ get();
            }

            /// <summary>
            /// Returns the globally unique identifier for this frame or &lt; 0 if the underlying frame does not yet exist.
            /// </summary>
            property Int64 Identifier
            {
                Int64 get();
            }

            /// <summary>
            /// Returns the parent of this frame or NULL if this is the main (top-level) frame.
            /// </summary>
            property IFrame^ Parent
            {
                IFrame^ get();
            }

            /// <summary>
            /// Returns the URL currently loaded in this frame.
            /// </summary>
            property String^ Url
            {
                String^ get();
            }

            /// <summary>
            /// Returns the browser that this frame belongs to.
            /// </summary>
            property IBrowser^ Browser
            {
                IBrowser^ get();
            }

            /// <summary>
            /// Gets a value indicating whether the frame has been disposed of.
            /// </summary>
            property bool IsDisposed
            {
                bool get();
            }
        };
    }
}
