using System;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows;
using Sticky.ViewModels;

namespace Sticky.Helpers {

  public static class WindowHelper {

    public static WINDOWPLACEMENT DeserializePlacementOrDefault(IntPtr handle, string json) {
      try {
        var placement = JsonSerializer.Deserialize<WINDOWPLACEMENT>(json);

        placement.Length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
        placement.Flags = 0;
        placement.ShowCmd = (placement.ShowCmd == Native.SW_SHOWMAXIMIZED) ? Native.SW_SHOWMAXIMIZED : Native.SW_SHOWNORMAL;
        return placement;
      } catch (Exception) { }

      _ = Native.GetWindowPlacement(handle, out var defaultPlacement);
      return defaultPlacement;
    }

    public static string SerializePlacement(IntPtr handle) {
      _ = Native.GetWindowPlacement(handle, out var placement);
      try {
        return JsonSerializer.Serialize(placement);
      } catch (Exception) { }
      return "";
    }

    public static NoteWindow? FindNoteWindow(WindowCollection allWindows, int noteId) {
      foreach (var window in allWindows) {
        if (window is NoteWindow nw && nw.ContainsNote(noteId)) {
          return nw;
        }
      }
      return null;
    }

    public static void CloseNoteWindow(WindowCollection allWindows, int noteId) {
      var window = WindowHelper.FindNoteWindow(allWindows, noteId);
      if (window != null) window.Close();
    }

    // @TODO: Find appropriate position for a note window.
    public static void OpenNoteWindow(NoteWindowViewModel vm, Window mainWindow) {
      var window = new NoteWindow(vm);

      #if false
      if (mainWindow != null) {
        window.Left = mainWindow.Left + mainWindow.Width + 12;
        window.Top = mainWindow.Top;
      }
      #endif

      window.Show();
    }

  }

}
