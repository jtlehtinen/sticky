using System;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace Sticky {

  // https://docs.microsoft.com/en-us/windows/apps/desktop/modernize/apply-rounded-corners
  public static class Native {
    public enum DWMWINDOWATTRIBUTE {
      DWMWA_WINDOW_CORNER_PREFERENCE = 33
    }

    public enum DWM_WINDOW_CORNER_PREFERENCE {
      DWMWCP_DEFAULT      = 0,
      DWMWCP_DONOTROUND   = 1,
      DWMWCP_ROUND        = 2,
      DWMWCP_ROUNDSMALL   = 3
    }

    [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
    internal static extern void DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attribute, ref DWM_WINDOW_CORNER_PREFERENCE pvAttribute, uint cbAttribute);

    public static void ApplyRoundedWindowCorners(Window window) {
      IntPtr handle = new WindowInteropHelper(window).EnsureHandle();

      var attribute = DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
      var preference = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
      DwmSetWindowAttribute(handle, attribute, ref preference, sizeof(uint));
    }
  }

}
