// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;

namespace CefSharp.Internals
{
    public static class MessagingExtensions
    {
        public static void SerializeJsRootObject(this IListValue list, JavascriptRootObject value)
        {
            var memberObjects = value.MemberObjects;

            list.SetInt(0, memberObjects.Count);

            for (var i = 0; i < memberObjects.Count; i++)
            {
                var subList = list.CreateList();
                subList.SerializeJsObject(memberObjects[i]);

                list.SetList(i + 1, subList);
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
                        var subList = list.CreateList();
                        subList.SerializeJsObject(property.JsObject);

                        list.SetList(i++, subList);
                    }
                }
            }
        }

        public static JavascriptRootObject DeserializeJsRootObject(this IListValue list)
        {
            var result = new JavascriptRootObject();
            var memberCount = list.GetInt(0);

            for (var i = 0; i < memberCount; i++)
            {
                var type = list.GetType(i + 1);
                if (type == CefValueType.List)
                {
                    var subList = list.GetList(i + 1);
                    var jsObject = subList.DeserializeJsObject();
                    result.MemberObjects.Add(jsObject);
                }
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

        public static object DeserializeV8Object(this IListValue list, int index, IJavascriptCallbackFactory javascriptCallbackFactory)
        {
            var type = list.GetType(index);

            if (type == CefValueType.Bool)
            {
                return list.GetBool(index);
            }

            if (type == CefValueType.Int)
            {
                return list.GetInt(index);
            }

            if (type == CefValueType.Double)
            {
                return list.GetDouble(index);
            }

            if (type == CefValueType.String)
            {
                return list.GetString(index);
            }

            if (type == CefValueType.List)
            {
                var subList = list.GetList(index);
                var array = new List<Object>((int)subList.GetSize());
                for (var i = 0; i < array.Capacity; i++)
                {
                    array.Add(subList.DeserializeV8Object(i, javascriptCallbackFactory));
                }
                return array;
            }

            if (type == CefValueType.Dictionary)
            {
                throw new NotImplementedException();
                //var dict = new Dictionary<String, Object>();
                //var subDict = list.GetDictionary(index);
                //std::vector<CefString> keys;
                //subDict.GetKeys(keys);

                //for (auto i = 0; i < keys.size(); i++)
                //{
                //	dict->Add(StringUtils::ToClr(keys[i]), DeserializeV8Object(subDict, keys[i], javascriptCallbackFactory));
                //}

                //result = dict;
            }

            if (type == CefValueType.Binary)
            {
                var binary = list.GetBinary(index);

                var t = (PrimitiveType)binary[0];

                if (t == PrimitiveType.Int64)
                {
                    return BitConverter.ToInt64(binary, 1);
                }

                if (t == PrimitiveType.CefTime)
                {
                    var epoch = BitConverter.ToDouble(binary, 1);

                    return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(epoch).ToLocalTime();
                }

                if (t == PrimitiveType.JsCallback)
                {
                    var browserId = new byte[sizeof(int)];

                    //Copy the middle bytes out that represent Browser Id
                    Array.Copy(binary, 1, browserId, 0, browserId.Length);

                    var result = new JavascriptCallback
                    {
                        BrowserId = BitConverter.ToInt32(browserId, 0),
                        Id = BitConverter.ToInt64(binary, 1 + sizeof(int))
                    };

                    return javascriptCallbackFactory.Create(result);
                }
            }

            throw new NotImplementedException();
        }

        public static bool SerializeObject(this IListValue list, int index, object obj)
        {
            if(obj == null)
            {
                list.SetNull(index);

                return true;
            }

            var type = obj.GetType();

            var underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
            {
                type = underlyingType;
            }

            if (type == typeof(bool))
            {
                list.SetBool(index, (bool)obj);

                return true;
            }

            if (type == typeof(int))
            {
                list.SetInt(index, (int)obj);

                return true;
            }

            if (type == typeof(string))
            {
                list.SetString(index, (string)obj);

                return true;
            }
            
            //if (type == Double::typeid)
            //{
            //	return CefV8Value::CreateDouble(safe_cast<double>(obj));
            //}
            //if (type == Decimal::typeid)
            //{
            //	return CefV8Value::CreateDouble(Convert::ToDouble(obj));
            //}
            //if (type == SByte::typeid)
            //{
            //	return CefV8Value::CreateInt(Convert::ToInt32(obj));
            //}
            //if (type == Int16::typeid)
            //{
            //	return CefV8Value::CreateInt(Convert::ToInt32(obj));
            //}
            //if (type == Int64::typeid)
            //{
            //	return CefV8Value::CreateDouble(Convert::ToDouble(obj));
            //}
            //if (type == Byte::typeid)
            //{
            //	return CefV8Value::CreateInt(Convert::ToInt32(obj));
            //}
            //if (type == UInt16::typeid)
            //{
            //	return CefV8Value::CreateInt(Convert::ToInt32(obj));
            //}
            //if (type == UInt32::typeid)
            //{
            //	return CefV8Value::CreateDouble(Convert::ToDouble(obj));
            //}
            //if (type == UInt64::typeid)
            //{
            //	return CefV8Value::CreateDouble(Convert::ToDouble(obj));
            //}
            //if (type == Single::typeid)
            //{
            //	return CefV8Value::CreateDouble(Convert::ToDouble(obj));
            //}
            //if (type == Char::typeid)
            //{
            //	return CefV8Value::CreateInt(Convert::ToInt32(obj));
            //}
            //if (type == DateTime::typeid)
            //{
            //	return CefV8Value::CreateDate(TypeUtils::ConvertDateTimeToCefTime(safe_cast<DateTime>(obj)));
            //}
            //if (type->IsArray)
            //{
            //	Array^ managedArray = (Array^)obj;
            //	CefRefPtr<CefV8Value> cefArray = CefV8Value::CreateArray(managedArray->Length);

            //	for (int i = 0; i < managedArray->Length; i++)
            //	{
            //		Object^ arrObj;

            //		arrObj = managedArray->GetValue(i);

            //		if (arrObj != nullptr)
            //		{
            //			CefRefPtr<CefV8Value> cefObj = TypeUtils::ConvertToCef(arrObj, arrObj->GetType());

            //			cefArray->SetValue(i, cefObj);
            //		}
            //		else
            //		{
            //			cefArray->SetValue(i, CefV8Value::CreateNull());
            //		}
            //	}

            //	return cefArray;
            //}
            //if (type->IsValueType && !type->IsPrimitive && !type->IsEnum)
            //{
            //	cli::array<System::Reflection::FieldInfo^>^ fields = type->GetFields();
            //	CefRefPtr<CefV8Value> cefArray = CefV8Value::CreateArray(fields->Length);

            //	for (int i = 0; i < fields->Length; i++)
            //	{
            //		String^ fieldName = fields[i]->Name;

            //		CefString strFieldName = StringUtils::ToNative(safe_cast<String^>(fieldName));

            //		Object^ fieldVal = fields[i]->GetValue(obj);

            //		if (fieldVal != nullptr)
            //		{
            //			CefRefPtr<CefV8Value> cefVal = TypeUtils::ConvertToCef(fieldVal, fieldVal->GetType());

            //			cefArray->SetValue(strFieldName, cefVal, V8_PROPERTY_ATTRIBUTE_NONE);
            //		}
            //		else
            //		{
            //			cefArray->SetValue(strFieldName, CefV8Value::CreateNull(), V8_PROPERTY_ATTRIBUTE_NONE);
            //		}
            //	}

            //	return cefArray;
            //}
            ////TODO: What exception type?
            throw new NotImplementedException(string.Format("Cannot convert '{0}' object from CLR to CEF.", type.FullName));
        }
    }
}
