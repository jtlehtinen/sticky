using System;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace Sticky {

  // RECT structure required by WINDOWPLACEMENT structure
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public readonly record struct Rect(int Left, int Top, int Right, int Bottom) { }

  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public readonly record struct Point(int X, int Y) { }

  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public struct WindowPlacement {
    public int Length;
    public int Flags;
    public int ShowCmd;
    public Point MinPosition;
    public Point MaxPosition;
    public Rect NormalPosition;
  }

  // https://docs.microsoft.com/en-us/windows/apps/desktop/modernize/apply-rounded-corners
  public static class Native {
    public enum DWMWINDOWATTRIBUTE {
      DWMWA_WINDOW_CORNER_PREFERENCE = 33
    }

    public enum DWM_WINDOW_CORNER_PREFERENCE {
      DWMWCP_DEFAULT    = 0,
      DWMWCP_DONOTROUND = 1,
      DWMWCP_ROUND      = 2,
      DWMWCP_ROUNDSMALL = 3
    }

    [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
    internal static extern void DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attribute, ref DWM_WINDOW_CORNER_PREFERENCE pvAttribute, uint cbAttribute);

    public static void ApplyRoundedWindowCorners(Window window) {
      IntPtr handle = new WindowInteropHelper(window).EnsureHandle();

      var attribute = DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
      var preference = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
      DwmSetWindowAttribute(handle, attribute, ref preference, sizeof(uint));
    }

    [DllImport("user32.dll")]
    private static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WindowPlacement placement);

    [DllImport("user32.dll")]
    private static extern bool GetWindowPlacement(IntPtr hWnd, out WindowPlacement placement);

    public const int SwShownormal = 1;
    public const int SwShowminimized = 2;
  }

}
