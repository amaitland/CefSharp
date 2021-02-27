// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_frame.h"
#include "IFrame.h"
#include "IBrowser.h"

using namespace System::Threading::Tasks;

namespace CefSharp
{
    namespace RenderProcess
    {
        ///
        // Class used to represent a frame in the browser window. When used in the
        // browser process the methods of this class may be called on any thread unless
        // otherwise indicated in the comments. When used in the render process the
        // methods of this class may only be called on the main thread.
        ///
        /*--cef(source=library)--*/
        private ref class Frame : public IFrame
        {
        private:
            MCefRefPtr<CefFrame> _frame;
            CefSharp::RenderProcess::IFrame^ _parentFrame;
            CefSharp::RenderProcess::IBrowser^ _owningBrowser;
            Object^ _syncRoot;
            bool _disposed;

        internal:
            Frame(CefRefPtr<CefFrame> &frame)
                : _frame(frame), _parentFrame(nullptr),
                _owningBrowser(nullptr), _syncRoot(gcnew Object())
            {
            }

            !Frame()
            {
                _frame = NULL;
            }

            ~Frame()
            {
                this->!Frame();

                delete _parentFrame;
                delete _owningBrowser;
                _parentFrame = nullptr;
                _owningBrowser = nullptr;
                _syncRoot = nullptr;
                _disposed = true;
            }

        public:
            ///
            // True if this object is currently attached to a valid frame.
            ///
            /*--cef()--*/
            virtual property bool IsValid
            {
                bool get();
            }

            ///
            // Execute undo in this frame.
            ///
            /*--cef()--*/
            virtual void Undo();

            ///
            // Execute redo in this frame.
            ///
            /*--cef()--*/
            virtual void Redo();

            ///
            // Execute cut in this frame.
            ///
            /*--cef()--*/
            virtual void Cut();

            ///
            // Execute copy in this frame.
            ///
            /*--cef()--*/
            virtual void Copy();

            ///
            // Execute paste in this frame.
            ///
            /*--cef()--*/
            virtual void Paste();

            ///
            // Execute delete in this frame.
            ///
            /*--cef(capi_name=del)--*/
            virtual void Delete();

            ///
            // Execute select all in this frame.
            ///
            /*--cef()--*/
            virtual void SelectAll();

            ///
            // Load the specified |url|.
            ///
            /*--cef()--*/
            virtual void LoadUrl(String^ url);

            ///
            // Execute a string of JavaScript code in this frame. The |script_url|
            // parameter is the URL where the script in question can be found, if any.
            // The renderer may request this URL to show the developer the source of the
            // error.  The |start_line| parameter is the base line number to use for error
            // reporting.
            ///
            /*--cef(optional_param=script_url)--*/
            virtual void ExecuteJavaScriptAsync(String^ code, String^ scriptUrl, int startLine);

            ///
            // Returns true if this is the main (top-level) frame.
            ///
            /*--cef()--*/
            virtual property bool IsMain
            {
                bool get();
            }

            ///
            // Returns true if this is the focused frame.
            ///
            /*--cef()--*/
            virtual property bool IsFocused
            {
                bool get();
            }

            ///
            // Returns the name for this frame. If the frame has an assigned name (for
            // example, set via the iframe "name" attribute) then that value will be
            // returned. Otherwise a unique name will be constructed based on the frame
            // parent hierarchy. The main (top-level) frame will always have an empty name
            // value.
            ///
            /*--cef()--*/
            virtual property String^ Name
            {
                String^ get();
            }

            ///
            // Returns the globally unique identifier for this frame.
            ///
            /*--cef()--*/
            virtual property Int64 Identifier
            {
                Int64 get();
            }

            ///
            // Returns the parent of this frame or NULL if this is the main (top-level)
            // frame.
            ///
            /*--cef()--*/
            virtual property CefSharp::RenderProcess::IFrame^ Parent
            {
                CefSharp::RenderProcess::IFrame^ get();
            }

            ///
            // Returns the URL currently loaded in this frame.
            ///
            /*--cef()--*/
            virtual property String^ Url
            {
                String^ get();
            }

            ///
            // Returns the browser that this frame belongs to.
            ///
            /*--cef()--*/
            virtual property CefSharp::RenderProcess::IBrowser^ Browser
            {
                CefSharp::RenderProcess::IBrowser^ get();
            }

            virtual property bool IsDisposed
            {
                bool get();
            }
        };
    }
}
