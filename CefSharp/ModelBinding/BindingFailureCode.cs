// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.ModelBinding
{
    /// <summary>
    /// An enumeration of error codes for why the binding process failed.
    /// </summary>
    /// <remarks>
    /// The inline documentation mirrors the context attribute so it's possible to inspect failure context during development.
    /// Because XML documentation isn't going to be present at runtime in a Production release, we provide the attribute as well.
    /// </remarks>
    public enum BindingFailureCode
    {
        /// <summary>
        /// A default value for when no error code is provided.
        /// </summary>
        /// <remarks>
        /// We use "Unavailable" rather than "None" due to the fact "None" is misleading and can lead to improper error handling.
        /// </remarks>
        [BindingFailureContext("No failure code is available for this exception")]
        Unavailable,
        /// <summary>
        /// It was inferred the source object was an <see cref="Enum"/>. field, however the destination type is not an <see cref="Enum"/>.
        /// </summary>
        [BindingFailureContext("It was inferred the source object was an Enum field, however the destination type is not an Enum.")]
        NoEnumAtDestinationType,
        /// <summary>
        /// No field exist in the destination enumeration that matches the source integral value.
        /// </summary>
        [BindingFailureContext("No field exist in the destination enumeration that matches the source integral value.")]
        NumberNotDefinedInEnum,
        /// <summary>
        /// No field exist in the destination enumeration that matches the source string.
        /// </summary>
        [BindingFailureContext("No field exist in the destination enumeration that matches the source string.")]
        StringNotDefinedInEnum,
        /// <summary>
        /// The destination enumeration contains no fields.
        /// </summary>
        [BindingFailureContext("The destination enumeration contains no fields.")]
        DestinationEnumEmpty,
        /// <summary>
        /// A string could not be parsed to an underlying integral type compatible with an enum.
        /// </summary>
        [BindingFailureContext("A string could not be parsed to an underlying integral type compatible with an enum.")]
        EnumIntegralNotFound,
        /// <summary>
        /// A provided source object was null or an empty collection on a non-nullable destination type.
        /// </summary>
        [BindingFailureContext("A provided source object was null on a non-nullable destination type.")]
        SourceObjectNullOrEmpty,
        /// <summary>
        /// The underlying type for the source object cannot be assigned to the destination type or lacks a destination altogether.
        /// </summary>
        [BindingFailureContext("The underlying type for the source object cannot be assigned to the destination type or lacks a destination altogether.")]
        SourceNotAssignable,
        /// <summary>
        ///  The Javascript object member does not correspond to any member on the destination type
        /// </summary>
        [BindingFailureContext("The Javascript object member {0} does not correspond to any member on the destination type. Are your style conventions correct?")]
        MemberNotFound,
        /// <summary>
        ///  The source type cannot be serialized to a type that is safe for Javascript to use.
        /// </summary>
        [BindingFailureContext("The source type {0} cannot be serialized to a type that is safe for Javascript to use. {1}")]
        UnsupportedJavascriptType
    }
}
