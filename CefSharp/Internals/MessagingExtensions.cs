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
                list.SerializeJsObject(memberObjects[i],  i);
            }
        }

        public static void SerializeJsObject(this IListValue list, JavascriptObject value, int index)
        {
            var subList = list.CreateListAtIndex(index);

            var i = 0;
            subList.SetInt64(i++, value.Id);
            subList.SetString(i++, value.Name);
            subList.SetString(i++, value.JavascriptName);

            subList.SetInt(i++, value.Methods.Count);
            foreach(var method in value.Methods)
            {
                subList.SetInt64(i++, method.Id);
                subList.SetString(i++, method.JavascriptName);
                subList.SetString(i++, method.ManagedName);
                subList.SetInt(i++, method.ParameterCount);
            }

            subList.SetInt(i++, value.Properties.Count);
            foreach(var property in value.Properties)
            {
                subList.SetInt64(i++, property.Id);
                subList.SetString(i++, property.JavascriptName);
                subList.SetString(i++, property.ManagedName);
                subList.SetBool(i++, property.IsComplexType);
                subList.SetBool(i++, property.IsReadOnly);
                if (property.IsComplexType)
                {
                    if (property.JsObject == null)
                    {
                        subList.SetNull(i++);
                    }
                    else
                    {
                        subList.SerializeJsObject(property.JsObject, i++);
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
                result.MemberObjects.Add(list.DeserializeJsObject(i));
            }

            return result;
        }

        public static JavascriptObject DeserializeJsObject(this IListValue list, int index)
        {
            var result = new JavascriptObject();
            var subList = list.GetList(index);
            var i = 0;

            result.Id = subList.GetInt64(i++);
            result.Name = subList.GetString(i++);
            result.JavascriptName = subList.GetString(i++);

            var methodCount = subList.GetInt(i++);
            for (var j = 0; j < methodCount; j++)
            {
                var method = new JavascriptMethod();
                method.Id = subList.GetInt64(i++);
                method.JavascriptName = subList.GetString(i++);
                method.ManagedName = subList.GetString(i++);
                method.ParameterCount = subList.GetInt(i++);

                result.Methods.Add(method);
            }

            var propertyCount = subList.GetInt(i++);
            for (var j = 0; j < propertyCount; j++)
            {
                var prop = new JavascriptProperty();
                prop.Id = subList.GetInt64(i++);
                prop.JavascriptName = subList.GetString(i++);
                prop.ManagedName = subList.GetString(i++);
                prop.IsComplexType = subList.GetBool(i++);
                prop.IsReadOnly = subList.GetBool(i++);
                if (prop.IsComplexType)
                {
                    var type = subList.GetType(i);
                    if (type == CefValueType.List)
                    {
                        prop.JsObject = subList.DeserializeJsObject(i++);
                    }
                }

                result.Properties.Add(prop);
            }

            return result;
        }
    }
}
