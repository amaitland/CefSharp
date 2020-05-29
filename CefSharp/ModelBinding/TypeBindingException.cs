// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.ComponentModel;
using System.Linq;

namespace CefSharp.ModelBinding
{
    /// <summary>
    /// An exception that is thrown whenever data cannot be properly marshaled.
    /// </summary>
    public class TypeBindingException : Exception
    {
        /// <summary>
        /// The underlying type that was inferred for the source object that needed to be bound 
        /// </summary>
        public Type SourceObjectType { get; }
        public Type DestinationType { get; }
        public string Context { get; }
        public BindingFailureCode Code { get; }

        /// <summary>
        /// Creates a new <see cref="TypeBindingException"/> using a backing failure code for which context can be derived.
        /// </summary>
        /// <param name="sourceObjectType">the inferred type for the object that was meant to be bound.</param>
        /// <param name="destinationType">the destination type the object attempted to be marshaled to.</param>
        /// <param name="code">a failure code that defines the reason the binding process failed.</param>
        /// <param name="formatValues">if present, any values here will be used to format the context string.</param>
        public TypeBindingException(Type sourceObjectType, Type destinationType, BindingFailureCode code, params object[] formatValues)
        {
            SourceObjectType = sourceObjectType;
            DestinationType = destinationType;
            Code = code;
            Context = FindFailureContext(code);
            if (formatValues != null && formatValues.Length > 0)
            {
                Context = string.Format(Context, formatValues);
            }
        }

        /// <summary>
        /// Attempts to find the <see cref="BindingFailureContextAttribute"/> value of a given <see cref="BindingFailureCode"/> field.
        /// </summary>
        /// <param name="value">the field to find context on.</param>
        /// <returns>the value set on the <see cref="BindingFailureContextAttribute"/>, otherwise a default non-null value.</returns>
        private static string FindFailureContext(Enum value)
        {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);

            return enumType.GetField(name).GetCustomAttributes(false).OfType<DescriptionAttribute>().SingleOrDefault()?.Description ?? "No context is available this error code.";
        }

        /// <summary>
        /// Creates a new <see cref="TypeBindingException"/> without a backing failure code.
        /// </summary>
        /// <param name="sourceObjectType">the inferred type for the object that was meant to be bound.</param>
        /// <param name="destinationType">the destination type the object attempted to be marshaled to.</param>
        /// <param name="context">in lieu of a failure code, provide a explanation as to why the binding process failed.</param>
        /// <remarks>
        /// the <see cref="Code"/> property will automatically be set to <see cref="BindingFailureCode.Unavailable"/>
        /// </remarks>
        public TypeBindingException(Type sourceObjectType, Type destinationType, string context)
        {
            SourceObjectType = sourceObjectType;
            DestinationType = destinationType;
            Context = context;
            Code = BindingFailureCode.Unavailable;
        }
    }
}
