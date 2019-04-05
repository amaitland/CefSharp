using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using CefSharp.Wpf.Internals;

namespace CefSharp.Wpf.Experimental
{
    /// <summary>
    /// Based on https://bitbucket.org/chromiumembedded/cef/pull-requests/214/cefclient-win-pointer-events/diff
    /// </summary>
    internal class WmPointerHandler : IHwndSourceObserver
    {
        //https://github.com/jaytwo/WmTouchDevice/blob/ec087db2ee58a9a1cbcd37a8484f41288a735fcc/WmTouchDevice/Native/NativeMethods.cs#L15
        private const uint TWF_WANTPALM = 0x00000002;

        //https://github.com/jaytwo/WmTouchDevice
        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool RegisterTouchWindow(IntPtr hWnd, uint ulFlags);

        //http://www.pinvoke.net/default.aspx/user32.RegisterTouchWindow
        //[DllImport("user32.dll", SetLastError = true)]
        //private static extern bool RegisterTouchWindow(IntPtr hWnd, int flags);

        //https://stackoverflow.com/a/45194187/4583726
        private static ushort LOWORD(ulong l)
        {
            return (ushort)(l & 0xFFFF);
        }

        private static ushort HIWORD(ulong l)
        {
            return (ushort)((l >> 16) & 0xFFFF);
        }

        private static ushort GET_POINTERID_WPARAM(ulong wParam)
        {
            return LOWORD(wParam);
        }

        private static ushort GET_X_LPARAM(ulong lp)
        {
            return LOWORD(lp);
        }

        private static ushort GET_Y_LPARAM(ulong lp)
        {
            return HIWORD(lp);
        }

        /// <summary>
        /// The source hook		
        /// </summary>		
        private HwndSourceHook sourceHook;

        /// <summary>
        /// The source		
        /// </summary>		
        private HwndSource source;

        /// <summary>
        /// The owner browser instance
        /// </summary>
        private readonly ChromiumWebBrowser owner;

        public WmPointerHandler(ChromiumWebBrowser owner)
        {
            this.owner = owner;
        }

        public virtual void NotifySourceChange(HwndSource source)
        {
            ReleaseHook();

            this.source = source;
            if (source != null)
            {
                sourceHook = SourceHook;
                source.AddHook(SourceHook);

                //We may want to specify TWF_WANTPALM
                //https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-registertouchwindow
                //RegisterTouchWindow(source.Handle, TWF_WANTPALM);
                RegisterTouchWindow(source.Handle, 0);
            }
        }

        public virtual void Dispose()
        {
            ReleaseHook();
        }

        private void ReleaseHook()
        {
            if (source != null && sourceHook != null)
            {
                source.RemoveHook(sourceHook);
            }
            source = null;
        }

        /// <summary>		
        /// WindowProc callback interceptor. Handles Windows messages intended for the source hWnd, and passes them to the		
        /// contained browser as needed.		
        /// </summary>		
        /// <param name="hWnd">The source handle.</param>		
        /// <param name="message">The message.</param>		
        /// <param name="wParam">Additional message info.</param>		
        /// <param name="lParam">Even more message info.</param>		
        /// <param name="handled">if set to <c>true</c>, the event has already been handled by someone else.</param>		
        /// <returns>IntPtr.</returns>		
        protected virtual IntPtr SourceHook(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (handled)
            {
                return IntPtr.Zero;
            }

            switch ((WM)message)
            {
                // Use pointer events for touch and pen on Windows 8 and newer.
                case WM.PointerDown:
                case WM.PointerUpdate:
                case WM.PointerUp:
                {
                    if (OnPointerEvent(message, wParam, lParam))
                    {
                        return IntPtr.Zero;
                    }

                    break;
                }
            }

            return IntPtr.Zero;
        }

        private bool OnPointerEvent(int message, IntPtr wParam, IntPtr lParam)
        {
            var id = GET_POINTERID_WPARAM((ulong)wParam);
            var inputCount = wParam.ToInt32() & 0xffff;

            return false;
            //          union {
            //              POINTER_TOUCH_INFO touch_info;
            //              POINTER_PEN_INFO pen_info;
            //              POINTER_INFO pointer_info;
            //          };

            //          CefTouchEvent event;
            //event.id = id;

            //if (type == PT_PEN && pointer_events::GetPointerPenInfo(id, &pen_info)) {
            //  if (pen_info.penFlags & PEN_FLAG_ERASER) {
            //    event.pointer_type = CEF_POINTER_TYPE_ERASER;
            //      } else {
            //    event.pointer_type = CEF_POINTER_TYPE_PEN;
            //      }

            //  if (pen_info.penMask & PEN_MASK_PRESSURE)
            //    event.pressure = pen_info.pressure / 1024.f;

            //  if (pen_info.penMask & PEN_MASK_ROTATION)
            //    event.rotation_angle = pen_info.rotation / 180.f * 3.14159f;
            //      } else if (type == PT_TOUCH &&
            //           pointer_events::GetPointerTouchInfo(id, &touch_info)) {
            //  event.pointer_type = CEF_POINTER_TYPE_TOUCH;
            //      } else {
            //  return false;
            //}

            //// Ignore hover events.
            //if (message == WM_POINTERUPDATE &&
            //    (pointer_info.pointerFlags & POINTER_FLAG_INCONTACT) == 0) {
            //  return true;
            //}

            //POINT point = pointer_info.ptPixelLocation;
            //      ScreenToClient(hwnd_, &point);

            //      event.x = DeviceToLogical(point.x, device_scale_factor_);
            //event.y = DeviceToLogical(point.y, device_scale_factor_);

            //if (message == WM_POINTERDOWN)
            //  event.type = CEF_TET_PRESSED;
            //else if (message == WM_POINTERUP)
            //  event.type = CEF_TET_RELEASED;
            //else
            //  event.type = CEF_TET_MOVED;

            //if (pointer_info.pointerFlags & POINTER_FLAG_CANCELED)
            //  event.type = CEF_TET_CANCELLED;

            //if (browser_)
            //  browser_->GetHost()->SendTouchEvent(event);
        }
    }
}
