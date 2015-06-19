// Copyright � 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CefBrowserHostWrapper.h"


CefRefPtr<CefClient> CefBrowserHostWrapper::GetClient()
{
    return _browserHost->GetClient();
}

void CefBrowserHostWrapper::StartDownload(String^ url)
{
    _browserHost->StartDownload(StringUtils::ToNative(url));
}

void CefBrowserHostWrapper::Print()
{
    _browserHost->Print();
}

void CefBrowserHostWrapper::SetZoomLevel(double zoomLevel)
{
    _browserHost->SetZoomLevel(zoomLevel);
}

Task<double>^ CefBrowserHostWrapper::GetZoomLevelAsync()
{
    if (CefCurrentlyOn(TID_UI))
    {
        TaskCompletionSource<double>^ taskSource = gcnew TaskCompletionSource<double>();
        taskSource->SetResult(GetZoomLevelOnUI());
        return taskSource->Task;
    }
    return Cef::UIThreadTaskFactory->StartNew(gcnew Func<double>(this, &CefBrowserHostWrapper::GetZoomLevelOnUI));
}

IntPtr CefBrowserHostWrapper::GetWindowHandle()
{
    return IntPtr(_browserHost->GetWindowHandle());
}

void CefBrowserHostWrapper::CloseBrowser(bool forceClose)
{
    _browserHost->CloseBrowser(forceClose);
}

void CefBrowserHostWrapper::ShowDevTools()
{
    CefWindowInfo windowInfo;
    CefBrowserSettings settings;

    windowInfo.SetAsPopup(_browserHost->GetWindowHandle(), "DevTools");

    _browserHost->ShowDevTools(windowInfo, _browserHost->GetClient(), settings, CefPoint());
}

void CefBrowserHostWrapper::CloseDevTools()
{
    _browserHost->CloseDevTools();
}

void CefBrowserHostWrapper::AddWordToDictionary(String^ word)
{
    _browserHost->AddWordToDictionary(StringUtils::ToNative(word));
}

void CefBrowserHostWrapper::ReplaceMisspelling(String^ word)
{
    _browserHost->ReplaceMisspelling(StringUtils::ToNative(word));
}

void CefBrowserHostWrapper::Find(int identifier, String^ searchText, bool forward, bool matchCase, bool findNext)
{
    _browserHost->Find(identifier, StringUtils::ToNative(searchText), forward, matchCase, findNext);
}

void CefBrowserHostWrapper::StopFinding(bool clearSelection)
{
    _browserHost->StopFinding(clearSelection);
}

double CefBrowserHostWrapper::GetZoomLevelOnUI()
{
    CefTaskScheduler::EnsureOn(TID_UI, "CefBrowserHostWrapper::GetZoomLevel");

    //Don't throw exception if no browser host here as it's not easy to handle
    if(_browserHost.get())
    {
        return _browserHost->GetZoomLevel();
    }

    return 0.0;	
}