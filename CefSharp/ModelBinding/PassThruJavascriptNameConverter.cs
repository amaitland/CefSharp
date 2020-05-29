// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.ModelBinding
{
    /// <summary>
    /// Javascript Name converter that makes no change to the .Net name
    /// </summary>
    public class PassThruJavascriptNameConverter : IJavascriptNameConverter
    {
        string IJavascriptNameConverter.ConvertToJavascript(string name)
        {
            return name;
        }
    }
}
