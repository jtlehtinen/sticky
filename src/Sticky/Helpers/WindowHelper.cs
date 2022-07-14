using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Interop;
using Sticky.Extensions;
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

    public static string SerializePlacement(Window window) {
      var handle = new WindowInteropHelper(window).Handle;

      _ = Native.GetWindowPlacement(handle, out var placement);
      try {
        return JsonSerializer.Serialize(placement);
      } catch (Exception) { }
      return "";
    }

    private static void SetStoredWindowPlacementOrDefault(Window window, NoteWindowViewModel viewModel) {
      if (string.IsNullOrWhiteSpace(viewModel.WindowPosition)) return;

      var handle = new WindowInteropHelper(window).Handle;
      var placement = WindowHelper.DeserializePlacementOrDefault(handle, viewModel.WindowPosition);

      Native.SetWindowPlacement(handle, ref placement);
    }

    public static NoteWindow? FindNoteWindow(WindowCollection windows, int noteId) {
      return (NoteWindow)windows.Find((window) => window is NoteWindow nw && nw.ContainsNote(noteId));
    }

    public static void CloseNoteWindow(WindowCollection allWindows, int noteId) {
      var window = WindowHelper.FindNoteWindow(allWindows, noteId);
      if (window != null) window.Close();
    }

    public static void OpenNoteWindow(WindowCollection windows, NoteWindowViewModel vm, Window positionRelativeTo) {
      var window = new NoteWindow(vm);

      if (positionRelativeTo != null) {
        WindowPositioner.Position(windows, positionRelativeTo, window);
      }

      SetStoredWindowPlacementOrDefault(window, vm);

      window.Show();
      window.Activate();
      window.TakeKeyboardFocus();
    }

    private static int IndexOf(WindowCollection collection, Window what) {
      var count = collection.Count;
      for (var i = 0; i < count; ++i) {
        if (collection[i] == what) return i;
      }
      return -1;
    }

    private static void ActivateAndTakeKeyboardFocus(Window window) {
      window.Activate();
      if (window is NoteWindow nw) {
        nw.TakeKeyboardFocus();
      }
    }

    public static void ActivateNextWindow(WindowCollection allWindows, Window from) {
      var index = IndexOf(allWindows, from);
      if (index == -1) return;

      index = (index + 1) % allWindows.Count;
      ActivateAndTakeKeyboardFocus(allWindows[index]);
    }

    public static void ActivatePreviousWindow(WindowCollection allWindows, Window from) {
      var index = IndexOf(allWindows, from);
      if (index == -1) return;

      index = (index - 1 + allWindows.Count) % allWindows.Count;
      ActivateAndTakeKeyboardFocus(allWindows[index]);
    }

    public static Window? FindFocusedWindow(WindowCollection windows) {
      return
        windows.Find((window) => window.IsFocused) ??
        windows.Find((window) => window.IsKeyboardFocusWithin);
    }
  }

}
