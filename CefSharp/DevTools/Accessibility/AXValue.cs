// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Accessibility
{
    /// <summary>
    /// A single computed AX property.
    /// </summary>
    public class AXValue : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The type of this value.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("type"), IsRequired = (true))]
        public CefSharp.DevTools.Accessibility.AXValueType Type
        {
            get;
            set;
        }

        /// <summary>
        /// The computed value of this property.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("value"), IsRequired = (false))]
        public object Value
        {
            get;
            set;
        }

        /// <summary>
        /// One or more related nodes, if applicable.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("relatedNodes"), IsRequired = (false))]
        public System.Collections.Generic.IList<CefSharp.DevTools.Accessibility.AXRelatedNode> RelatedNodes
        {
            get;
            set;
        }

        /// <summary>
        /// The sources which contributed to the computation of this property.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("sources"), IsRequired = (false))]
        public System.Collections.Generic.IList<CefSharp.DevTools.Accessibility.AXValueSource> Sources
        {
            get;
            set;
        }
    }
}