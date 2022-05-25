using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Sticky {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    private List<Window> noteWindows = new List<Window>();

    public static RoutedCommand ChangeThemeCommand = new RoutedCommand();

    public MainWindow() {
      InitializeComponent();
      Native.ApplyRoundedWindowCorners(this);
      ((App)Application.Current).SetTheme("Themes.Blue");

      var binding = new CommandBinding(ChangeThemeCommand, ExecutedChangeThemeCommand, CanExecuteChangeThemeCommand);
      CommandBindings.Add(binding);
    }

    private void ExecutedChangeThemeCommand(object sender, ExecutedRoutedEventArgs e) {
      var parameter = (string)e.Parameter;
      ((App)Application.Current).SetTheme(parameter);
    }

    private void CanExecuteChangeThemeCommand(object sender, CanExecuteRoutedEventArgs e) {
      e.CanExecute = true;
    }

    public void OnAddNote() {
      noteWindows.Add(new NoteWindow());
    }

    public void OnRemoveNote(NoteWindow window) {
      noteWindows.Remove(window);
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs args) {
      // @NOTE: This hack is here because:
      // Given: AllowsTransparency="True"
      // When: Window maximized
      // Then: Window size is greater than the monitor size... WTF?

      var thickness = (this.WindowState == WindowState.Maximized ? 8 : 0);
      this.BorderThickness = new System.Windows.Thickness(thickness);
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs args) {
      noteWindows.ForEach(window => window.Close());
      noteWindows.Clear();
      base.OnClosing(args);
    }
  }
}
