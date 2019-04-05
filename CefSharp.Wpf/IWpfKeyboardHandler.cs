// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows.Input;

namespace CefSharp.Wpf
{
    /// <summary>
    /// Implement this interface to control how keys are forwarded to the browser
    /// </summary>
    public interface IWpfKeyboardHandler
    {
        void HandleKeyPress(KeyEventArgs e);
        void HandleTextInput(TextCompositionEventArgs e);
    }
}
