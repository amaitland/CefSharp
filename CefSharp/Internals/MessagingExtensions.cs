// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Internals
{
    public static class MessagingExtensions
    {
        public static void SerializeJsRootObject(this IListValue list, JavascriptRootObject value)
        {
            var memberObjects = value.MemberObjects;
            for (var i = 0; i < memberObjects.Count; i++)
            {
                SerializeJsObject(list, memberObjects[i],  i);
            }
        }

        public static void SerializeJsObject(this IListValue list, JavascriptObject value, int index)
        {
            var val = list.CreateListAtIndex(index);

            var i = 0;
            val.SetInt64(i++, value.Id);
            val.SetString(i++, value.Name);
            val.SetString(i++, value.JavascriptName);

            val.SetInt(i++, value.Methods.Count);
            foreach(var method in value.Methods)
            {
                val.SetInt64(i++, method.Id);
                val.SetString(i++, method.JavascriptName);
                val.SetString(i++, method.ManagedName);
                val.SetInt(i++, method.ParameterCount);
            }

            val.SetInt(i++, value.Properties.Count);
            foreach(var property in value.Properties)
            {
                val.SetInt64(i++, property.Id);
                val.SetString(i++, property.JavascriptName);
                val.SetString(i++, property.ManagedName);
                val.SetBool(i++, property.IsComplexType);
                val.SetBool(i++, property.IsReadOnly);
                if (property.IsComplexType)
                {
                    if (property.JsObject != null)
                    {
                        val.SerializeJsObject(property.JsObject, i++);
                    }
                    else
                    {
                        val.SetNull(i++);
                    }
                }
            }
        }
    }
}
