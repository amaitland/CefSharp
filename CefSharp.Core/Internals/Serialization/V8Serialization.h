// Copyright � 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

namespace CefSharp
{
    namespace Internals
    {
        namespace Serialization
        {
            //Functions to deserialize data returned from the render process.

            //Deserializes data into Object from a given index of a CefListValue or CefDictionaryValue
            //IJavascriptCallbackFactory implementation should be passed to allow creation of valid javascript callbacks
            template<typename TList, typename TIndex>
            Object^ DeserializeV8Object(CefRefPtr<TList> list, TIndex index, IJavascriptCallbackFactory^ javascriptCallbackFactory);

            //Converts CefTime to DateTime
            DateTime ConvertCefTimeToDateTime(CefTime time);
        }
    }
}