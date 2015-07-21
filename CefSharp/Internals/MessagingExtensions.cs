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
                var subList = list.CreateListAtIndex(i);
                subList.SerializeJsObject(memberObjects[i]);
            }
        }

        public static void SerializeJsObject(this IListValue list, JavascriptObject value)
        {
            var i = 0;
            list.SetInt64(i++, i++, value.Id);
            list.SetString(i++, value.Name);
            list.SetString(i++, value.JavascriptName);

            list.SetInt(i++, value.Methods.Count);
            foreach(var method in value.Methods)
            {
                list.SetInt64(i++, i++, method.Id);
                list.SetString(i++, method.JavascriptName);
                list.SetString(i++, method.ManagedName);
                list.SetInt(i++, method.ParameterCount);
            }

            list.SetInt(i++, value.Properties.Count);
            foreach(var property in value.Properties)
            {
                list.SetInt64(i++, i++, property.Id);
                list.SetString(i++, property.JavascriptName);
                list.SetString(i++, property.ManagedName);
                list.SetBool(i++, property.IsComplexType);
                list.SetBool(i++, property.IsReadOnly);
                if (property.IsComplexType)
                {
                    if (property.JsObject == null)
                    {
                        list.SetNull(i++);
                    }
                    else
                    {
                        var subList = list.CreateListAtIndex(i++);
                        subList.SerializeJsObject(property.JsObject);
                    }
                }
            }
        }

        public static JavascriptRootObject DeserializeJsRootObject(this IListValue list)
        {
            var result = new JavascriptRootObject();
            var memberCount = (int)list.GetSize();

            for (var i = 0; i < memberCount; i++)
            {
                var subList = list.GetList(i);
                var jsObject = subList.DeserializeJsObject();
                result.MemberObjects.Add(jsObject);
            }

            return result;
        }

        public static JavascriptObject DeserializeJsObject(this IListValue list)
        {
            var result = new JavascriptObject();
            var i = 0;

            result.Id = list.GetInt64(i++, i++);
            result.Name = list.GetString(i++);
            result.JavascriptName = list.GetString(i++);

            var methodCount = list.GetInt(i++);
            for (var j = 0; j < methodCount; j++)
            {
                var method = new JavascriptMethod();
                method.Id = list.GetInt64(i++, i++);
                method.JavascriptName = list.GetString(i++);
                method.ManagedName = list.GetString(i++);
                method.ParameterCount = list.GetInt(i++);

                result.Methods.Add(method);
            }

            var propertyCount = list.GetInt(i++);
            for (var j = 0; j < propertyCount; j++)
            {
                var prop = new JavascriptProperty();
                prop.Id = list.GetInt64(i++, i++);
                prop.JavascriptName = list.GetString(i++);
                prop.ManagedName = list.GetString(i++);
                prop.IsComplexType = list.GetBool(i++);
                prop.IsReadOnly = list.GetBool(i++);
                if (prop.IsComplexType)
                {
                    var type = list.GetType(i);
                    if (type == CefValueType.List)
                    {
                        var subList = list.GetList(i);
                        prop.JsObject = subList.DeserializeJsObject();
                    }
                }

                result.Properties.Add(prop);
            }

            return result;
        }
    }
}
