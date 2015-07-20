// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using CefSharp.Internals;

namespace CefSharp.BrowserSubprocess.Messaging
{
    internal static class CefV8ValueExtensions
    {
        internal static void SerializeV8Object(this CefV8ValueWrapper obj, IListValue list, int index, JavascriptCallbackRegistry callbackRegistry, CefV8ContextWrapper context)
        {
            try
            {
                var seen = new List<CefV8ValueWrapper>();
                SerializeV8Object(obj, list, index, callbackRegistry, seen, context);
            }
            catch (Exception ex)
            {
                list.SetNull(index);
            }
        }

        internal static void SerializeV8Object(this CefV8ValueWrapper obj, IListValue list, int index, JavascriptCallbackRegistry callbackRegistry, List<CefV8ValueWrapper> seen, CefV8ContextWrapper context)
        {
            foreach (var item in seen)
            {
                if (obj.IsSame(item))
                {
                    throw new Exception("Cycle found");
                }
            }
            seen.Add(obj);

            if (obj.IsNull || obj.IsUndefined)
            {
                list.SetNull(index);
            }
            else if (obj.IsBool)
            {
                list.SetBool(index, obj.GetBoolValue());
            }
            else if (obj.IsInt)
            {
                list.SetInt(index, obj.GetIntValue());
            }
            else if (obj.IsDouble)
            {
                list.SetDouble(index, obj.GetDoubleValue());
            }
            else if (obj.IsString)
            {
                list.SetString(index, obj.GetStringValue());
            }
            else if (obj.IsDate)
            {
                throw new NotImplementedException();
                //SetCefTime(obj.GetDateValue(), list, index);
            }
            else if (obj.IsArray)
            {
                throw new NotImplementedException();
                //int arrLength = obj.GetArrayLength();
                //std::vector<CefString> keys;
                //if (arrLength > 0 && obj.GetKeys(keys))
                //{
                //	auto array = CefListValue::Create();
                //	for (int i = 0; i < arrLength; i++)
                //	{
                //		SerializeV8Object(obj.GetValue(keys[i]), array, i, callbackRegistry, seen);
                //	}

                //	list.SetList(index, array);
                //}
                //else
                //{
                //	list.SetNull(index);
                //}
            }
            else if (obj.IsFunction)
            {
                //auto jsCallback = callbackRegistry.Register(context, obj);
                //SetJsCallback(jsCallback, list, index);
                throw new NotImplementedException();
            }
            else if (obj.IsObject)
            {
                //std::vector<CefString> keys;
                //if (obj.GetKeys(keys) && keys.size() > 0)
                //{
                //	auto result = CefDictionaryValue::Create();
                //	for (int i = 0; i < keys.size(); i++)
                //	{
                //		auto p_keyStr = StringUtils::ToClr(keys[i].ToString());
                //		if ((obj.HasValue(keys[i])) && (!p_keyStr.StartsWith("__")))
                //		{
                //			SerializeV8Object(obj.GetValue(keys[i]), result, keys[i], callbackRegistry, seen);
                //		}
                //	}
                //	list.SetDictionary(index, result);
                //}
                throw new NotImplementedException();
            }
            else
            {
                list.SetNull(index);
            }
            seen.RemoveAt(seen.Count - 1);
        }

        public static CefV8ValueWrapper DeserializeV8Object(this IListValue list, int index)
        {
            var type = list.GetType(index);

            if (type == CefValueType.Bool)
            {
                return new CefV8ValueWrapper(list.GetBool(index));
            }
            if (type == CefValueType.Int)
            {
                return new CefV8ValueWrapper(list.GetInt(index));
            }
            if (type == CefValueType.Double)
            {
                return new CefV8ValueWrapper(list.GetDouble(index));
            }
            if (type == CefValueType.String)
            {
                return new CefV8ValueWrapper(list.GetString(index));
            }
                //else if (IsCefTime(list, index))
                //	result = CefV8Value::CreateDate(GetCefTime(list, index));
            else if (type == CefValueType.List)
            {
                var subList = list.GetList(index);
                var size = (int)subList.GetSize();
                var result = CefV8ValueWrapper.CreateArray(size);
                for (var i = 0; i < size; i++)
                {
                    result.SetValue(i, DeserializeV8Object(subList, i));
                }
            }
            else if (type == CefValueType.Dictionary)
            {
                var subDict = list.GetDictionary(index);
                var size = (int)subDict.Size;
                IList<string> keys;
                subDict.GetKeys(out keys);

                var result = CefV8ValueWrapper.CreateArray(size);

                for (var i = 0; i < size; i++)
                {
                    result.SetValue(keys[i], DeserializeV8Object(subDict, keys[i]));
                }
            }

            return new CefV8ValueWrapper();
        }

        public static CefV8ValueWrapper DeserializeV8Object(this IDictionaryValue dictionaryValue, string key)
        {
            var type = dictionaryValue.GetType(key);

            if (type == CefValueType.Bool)
            {
                return new CefV8ValueWrapper(dictionaryValue.GetBool(key));
            }
            if (type == CefValueType.Int)
            {
                return new CefV8ValueWrapper(dictionaryValue.GetInt(key));
            }
            if (type == CefValueType.Double)
            {
                return new CefV8ValueWrapper(dictionaryValue.GetDouble(key));
            }
            if (type == CefValueType.String)
            {
                return new CefV8ValueWrapper(dictionaryValue.GetString(key));
            }
            //else if (IsCefTime(list, index))
            //	result = CefV8Value::CreateDate(GetCefTime(list, index));
            else if (type == CefValueType.List)
            {
                var subList = dictionaryValue.GetList(key);
                var size = (int)subList.GetSize();
                var result = CefV8ValueWrapper.CreateArray(size);
                for (var i = 0; i < size; i++)
                {
                    result.SetValue(i, DeserializeV8Object(subList, i));
                }
            }
            else if (type == CefValueType.Dictionary)
            {
                var subDict = dictionaryValue.GetDictionary(key);
                var size = (int)subDict.Size;
                IList<string> keys;
                subDict.GetKeys(out keys);

                var result = CefV8ValueWrapper.CreateArray(size);

                for (var i = 0; i < size; i++)
                {
                    result.SetValue(keys[i], DeserializeV8Object(subDict, keys[i]));
                }
            }

            return new CefV8ValueWrapper();
        }

        public static JavascriptRootObject DeserializeJsRootObject(this IListValue list)
        {
            var result = new JavascriptRootObject();
            var memberCount = (int)list.GetSize();

            for (var i = 0; i < memberCount; i++)
            {
                result.MemberObjects.Add(DeserializeJsObject(list, i));
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
                        prop.JsObject = DeserializeJsObject(subList, i++);
                    }
                }

                result.Properties.Add(prop);
            }

            return result;
        }

        /*
         * template<typename TList, typename TIndex>
            bool IsType(PrimitiveType type, CefRefPtr<TList> list, TIndex index)
            {
                auto result = list->GetType(index) == VTYPE_BINARY;
                if (result)
                {
                    underlying_type<PrimitiveType>::type typeRead;
                    auto binaryValue = list->GetBinary(index);
                    binaryValue->GetData(&typeRead, sizeof(underlying_type<PrimitiveType>::type), 0);
                    result = typeRead == static_cast<underlying_type<PrimitiveType>::type>(type);
                }
                return result;
            }

            template<typename TList, typename TIndex>
            void SetInt64(const int64 &value, CefRefPtr<TList> list, TIndex index)
            {
                unsigned char mem[1 + sizeof(int64)];
                mem[0] = static_cast<unsigned char>(PrimitiveType::Int64);
                memcpy(reinterpret_cast<void*>(mem + 1), &value, sizeof(int64));

                auto binaryValue = CefBinaryValue::Create(mem, sizeof(mem));
                list->SetBinary(index, binaryValue);
            }

            template<typename TList, typename TIndex>
            int64 GetInt64(CefRefPtr<TList> list, TIndex index)
            {
                int64 result;

                auto binaryValue = list->GetBinary(index);
                binaryValue->GetData(&result, sizeof(int64), 1);

                return result;
            }

            template<typename TList, typename TIndex>
            bool IsInt64(CefRefPtr<TList> list, TIndex index)
            {
                return IsType(PrimitiveType::Int64, list, index);
            }

            template<typename TList, typename TIndex>
            void SetCefTime(const CefTime &value, CefRefPtr<TList> list, TIndex index)
            {
                auto doubleT = value.GetDoubleT();
                unsigned char mem[1 + sizeof(double)];
                mem[0] = static_cast<unsigned char>(PrimitiveType::CefTime);
                memcpy(reinterpret_cast<void*>(mem + 1), &doubleT, sizeof(double));

                auto binaryValue = CefBinaryValue::Create(mem, sizeof(mem));
                list->SetBinary(index, binaryValue);
            }

            template<typename TList, typename TIndex>
            CefTime GetCefTime(CefRefPtr<TList> list, TIndex index)
            {
                double doubleT;

                auto binaryValue = list->GetBinary(index);
                binaryValue->GetData(&doubleT, sizeof(double), 1);

                return CefTime(doubleT);
            }

            template<typename TList, typename TIndex>
            bool IsCefTime(CefRefPtr<TList> list, TIndex index)
            {
                return IsType(PrimitiveType::CefTime, list, index);
            }
            template<typename TList, typename TIndex>
            void SetJsCallback(JavascriptCallback^ value, CefRefPtr<TList> list, TIndex index)
            {
                auto id = value->Id;
                auto browserId = value->BrowserId;

                unsigned char mem[1 + sizeof(int) + sizeof(int64)];
                mem[0] = static_cast<unsigned char>(PrimitiveType::JsCallback);
                memcpy(reinterpret_cast<void*>(mem + 1), &browserId, sizeof(int));
                memcpy(reinterpret_cast<void*>(mem + 1 + sizeof(int)), &id, sizeof(int64));

                auto binaryValue = CefBinaryValue::Create(mem, sizeof(mem));
                list->SetBinary(index, binaryValue);
            }

            template<typename TList, typename TIndex>
            JavascriptCallback^ GetJsCallback(CefRefPtr<TList> list, TIndex index)
            {
                auto result = gcnew JavascriptCallback();
                int64 id;
                int browserId;

                auto binaryValue = list->GetBinary(index);
                binaryValue->GetData(&browserId, sizeof(int), 1);
                binaryValue->GetData(&id, sizeof(int64), 1 + sizeof(int));

                result->Id = id;
                result->BrowserId = browserId;

                return result;
            }

            template<typename TList, typename TIndex>
            bool IsJsCallback(CefRefPtr<TList> list, TIndex index)
            {
                return IsType(PrimitiveType::JsCallback, list, index);
            }
         * void SerializeV8Object(CefRefPtr<CefV8Value> obj, CefRefPtr<TList> list, TIndex index, JavascriptCallbackRegistry^ callbackRegistry, value_deque &seen)
        {
            for (value_deque::const_iterator it = seen.begin(); it != seen.end(); ++it)
            {
                if (obj.IsSame(*it))
                {
                    throw exception("Cycle found");
                }
            }
            seen.push_back(obj);

            if (obj.IsNull() || obj.IsUndefined())
            {
                list.SetNull(index);
            }
            else if (obj.IsBool())
                list.SetBool(index, obj.GetBoolValue());
            else if (obj.IsInt())
                list.SetInt(index, obj.GetIntValue());
            else if (obj.IsDouble())
                list.SetDouble(index, obj.GetDoubleValue());
            else if (obj.IsString())
                list.SetString(index, obj.GetStringValue());
            else if (obj.IsDate())
                SetCefTime(obj.GetDateValue(), list, index);
            else if (obj.IsArray())
            {
                int arrLength = obj.GetArrayLength();
                std::vector<CefString> keys;
                if (arrLength > 0 && obj.GetKeys(keys))
                {
                    auto array = CefListValue::Create();
                    for (int i = 0; i < arrLength; i++)
                    {
                        SerializeV8Object(obj.GetValue(keys[i]), array, i, callbackRegistry, seen);
                    }

                    list.SetList(index, array);
                }
                else
                {
                    list.SetNull(index);
                }
            }
            else if (obj.IsFunction())
            {
                auto context = CefV8Context::GetCurrentContext();
                auto jsCallback = callbackRegistry.Register(context, obj);
                SetJsCallback(jsCallback, list, index);
            }
            else if (obj.IsObject())
            {
                std::vector<CefString> keys;
                if (obj.GetKeys(keys) && keys.size() > 0)
                {
                    auto result = CefDictionaryValue::Create();
                    for (int i = 0; i < keys.size(); i++)
                    {
                        auto p_keyStr = StringUtils::ToClr(keys[i].ToString());
                        if ((obj.HasValue(keys[i])) && (!p_keyStr.StartsWith("__")))
                        {
                            SerializeV8Object(obj.GetValue(keys[i]), result, keys[i], callbackRegistry, seen);
                        }
                    }
                    list.SetDictionary(index, result);
                }
            }
            else
            {
                list.SetNull(index);
            }
            seen.pop_back();
        }

        template<typename TList, typename TIndex>
        CefRefPtr<CefV8Value> DeserializeV8Object(CefRefPtr<TList> list, TIndex index)
        {
            auto type = list.GetType(index);
            auto result = CefV8Value::CreateNull();

            if (type == VTYPE_BOOL)
                result = CefV8Value::CreateBool(list.GetBool(index));
            else if (type == VTYPE_INT)
                result = CefV8Value::CreateInt(list.GetInt(index));
            else if (type == VTYPE_DOUBLE)
                result = CefV8Value::CreateDouble(list.GetDouble(index));
            else if (type == VTYPE_STRING)
                result = CefV8Value::CreateString(list.GetString(index));
            else if (IsCefTime(list, index))
                result = CefV8Value::CreateDate(GetCefTime(list, index));
            else if (type == VTYPE_LIST)
            {
                auto subList = list.GetList(index);
                auto size = subList.GetSize();
                result = CefV8Value::CreateArray(size);
                for (auto i = 0; i < size; i++)
                {
                    result.SetValue(i, DeserializeV8Object(subList, i));
                }
            }
            else if (type == VTYPE_DICTIONARY)
            {
                auto subDict = list.GetDictionary(index);
                auto size = subDict.GetSize();
                std::vector<CefString> keys;
                subDict.GetKeys(keys);
                result = CefV8Value::CreateArray(size);
                for (auto i = 0; i < size; i++)
                {
                    result.SetValue(keys[i], DeserializeV8Object(subDict, keys[i]), V8_PROPERTY_ATTRIBUTE_NONE);
                }
            }

            return result;
        }
         */
    }
}
