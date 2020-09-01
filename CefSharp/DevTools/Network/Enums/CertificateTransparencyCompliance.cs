// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Whether the request complied with Certificate Transparency policy.
    /// </summary>
    public enum CertificateTransparencyCompliance
    {
        Unknown,
        NotCompliant,
        Compliant
    }
}