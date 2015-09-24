// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.ComponentModel;
using System.Windows;

namespace CefSharp.Wpf
{
    internal class DisposableEventWrapper : IDisposable
    {
        public DependencyObject Source { get; private set; }
        public EventHandler Handler { get; private set; }
        public DependencyPropertyDescriptor Descriptor { get; private set; }

        public DisposableEventWrapper(DependencyObject source, DependencyProperty property, EventHandler handler)
        {
            Source = source;
            Handler = handler;

            Descriptor = DependencyPropertyDescriptor.FromProperty(property, Source.GetType());

            Descriptor.AddValueChanged(Source, Handler);
        }

        public void Dispose()
        {
            Descriptor.RemoveValueChanged(Source, Handler);
        }
    }
}