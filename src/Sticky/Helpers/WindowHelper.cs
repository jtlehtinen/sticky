using System;
using System.Windows;
using System.Windows.Input;
using ModernWpf;
using Sticky.DataAccess;
using Sticky.ViewModels;

namespace Sticky.Helpers {

  public static class WindowHelper {

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

      if (mainWindow != null) {
        window.Left = mainWindow.Left + mainWindow.Width + 12;
        window.Top = mainWindow.Top;
      }

      window.Show();
    }

  }

}
