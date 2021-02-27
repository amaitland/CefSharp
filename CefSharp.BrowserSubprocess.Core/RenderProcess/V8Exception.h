// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

namespace CefSharp
{
    namespace RenderProcess
    {
        /// <summary>
        /// Class representing a V8 exception.
        /// </summary>
        public ref class V8Exception
        {
        private:
            int _endColumn;
            int _endPosition;
            int _lineNumber;
            String^ _message;
            String^ _scriptResourceName;
            String^ _sourceLine;
            int _startColumn;
            int _startPosition;

        public:
            /// <summary>
            /// Returns the index within the line of the last character where the error occurred.
            /// </summary>
            /// <returns>Returns the index within the line of the last character where the error occurred.</returns>
            property int EndColumn
            {
                int get()
                {
                    return _endColumn;
                }
            }

            /// <summary>
            /// Returns the index within the script of the last character where the error occurred.
            /// </summary>
            /// <returns>Returns the index within the script of the last character where the error occurred.</returns>
            property int EndPosition
            {
                int get()
                {
                    return _endColumn;
                }
            }

            /// <summary>
            /// Returns the 1-based number of the line where the error occurred or 0 if the line number is unknown.
            /// </summary>
            /// <returns>Returns the 1-based number of the line where the error occurred or 0 if the line number is unknown.</returns>
            property int LineNumber
            {
                int get()
                {
                    return _endColumn;
                }
            }

            /// <summary>
            /// Returns the exception message.
            /// </summary>
            /// <returns>Returns the exception message.</returns>
            property String^ Message
            {
                String^ get()
                {
                    return _message;
                }
            }

            /// <summary>
            /// Returns the resource name for the script from where the function causing the error originates.
            /// </summary>
            /// <returns>Returns the resource name for the script from where the function causing the error originates.</returns>
            property String^ ScriptResourceName
            {
                String^ get()
                {
                    return _scriptResourceName;
                }
            }

            /// <summary>
            /// Returns the line of source code that the exception occurred within.
            /// </summary>
            /// <returns>Returns the line of source code that the exception occurred within.</returns>
            property String^ SourceLine
            {
                String^ get()
                {
                    return _sourceLine;
                }
            }

            /// <summary>
            /// Returns the index within the line of the first character where the error occurred.
            /// </summary>
            /// <returns>Returns the index within the line of the first character where the error occurred.</returns>
            property int StartColumn
            {
                int get()
                {
                    return _startColumn;
                }
            }

            /// <summary>
            /// Returns the index within the script of the first character where the error occurred.
            /// </summary>
            /// <returns>Returns the index within the script of the first character where the error occurred.</returns>
            property int StartPosition
            {
                int get()
                {
                    return _startPosition;
                }
            }

           V8Exception(int endColumn,
                    int endPosition,
                    int lineNumber,
                    String^ message,
                    String^ scriptResourceName,
                    String^ sourceLine,
                    int startColumn,
                    int startPosition)
            {
                _endColumn = endColumn;
                _endPosition = endPosition;
                _lineNumber = lineNumber;
                _message = message;
                _scriptResourceName = scriptResourceName;
                _sourceLine = sourceLine;
                _startColumn = startColumn;
                _startPosition = startPosition;
            }
        };
    }
}

