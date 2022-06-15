using System.Collections.Generic;
using System.Windows;

namespace Sticky {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {

    // @TODO: Remove windows when they are closed.
    private List<Window> noteWindows = new List<Window>();

    public MainWindow() {
      var noteService = App.Current.Services.GetService<NoteService>();
      if (noteService != null) DataContext = noteService.GetNotes();

      InitializeComponent();
      Native.ApplyRoundedWindowCorners(this);
    }

    public void OnAddNote() {
      noteWindows.Add(new NoteWindow());
    }

    public void OnRemoveNote(NoteWindow window) {
      noteWindows.Remove(window);
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e) {
      // @NOTE: This hack is here because:
      // Given: AllowsTransparency="True"
      // When: Window maximized
      // Then: Window size is greater than the monitor size... WTF?

      var thickness = (this.WindowState == WindowState.Maximized ? 8 : 0);
      this.BorderThickness = new System.Windows.Thickness(thickness);
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
      noteWindows.ForEach(window => window.Close());
      noteWindows.Clear();
      base.OnClosing(e);
    }
  }
}
