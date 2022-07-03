using System;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace Sticky {

  // WINDOWPLACEMENT code taken from MIT-licensed PowerToys.
  // https://github.com/microsoft/PowerToys

  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Interop")]
  public struct POINT {
    public int X { get; set; }
    public int Y { get; set; }

    public POINT(int x, int y) {
      X = x;
      Y = y;
    }
  }

  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Interop")]
  public struct RECT {
    public int Left { get; set; }
    public int Top { get; set; }
    public int Right { get; set; }
    public int Bottom { get; set; }

    public RECT(int left, int top, int right, int bottom) {
      Left = left;
      Top = top;
      Right = right;
      Bottom = bottom;
    }
  }

  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Interop")]
  public struct WINDOWPLACEMENT {
    public int Length { get; set; }
    public int Flags { get; set; }
    public int ShowCmd { get; set; }
    public POINT MinPosition { get; set; }
    public POINT MaxPosition { get; set; }
    public RECT NormalPosition { get; set; }
  }

  // https://docs.microsoft.com/en-us/windows/apps/desktop/modernize/apply-rounded-corners
  public static class Native {
    internal const int SW_SHOWNORMAL = 1;
    internal const int SW_SHOWMAXIMIZED = 3;
    internal const int SW_HIDE = 0;

    [DllImport("user32.dll")]
    internal static extern bool SetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

    [DllImport("user32.dll")]
    internal static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

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
  }

}
