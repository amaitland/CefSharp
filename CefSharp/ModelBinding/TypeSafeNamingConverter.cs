// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.


namespace CefSharp.ModelBinding
{
    public class TypeSafeNamingConverter : IJavascriptNameConverter
    {
        string IJavascriptNameConverter.ConvertToJavascript(string name)
        {
            // don't allow whitespace in property names.
            // because we use this in the actual binding process, we should be throwing and not allowing invalid entries.
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new TypeBindingException(typeof(string), typeof(string), BindingFailureCode.SourceObjectNullOrEmpty);
            }

            // camelCase says that if the string is only one character that it is preserved.
            if (name.Length == 1)
            {
                return name;
            }

            var firstHalf = name.Substring(0, 1);
            var remainingHalf = name.Substring(1);

            // camelCase says that if the entire string is uppercase to preserve it.
            if (char.IsUpper(firstHalf[0]) && char.IsUpper(remainingHalf[0]))
            {
                return name;
            }

            return firstHalf.ToLowerInvariant() + remainingHalf;
        }
    }
}
