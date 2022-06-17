using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sticky {

  public partial class NoteWindowTitleBar : UserControl {
    public NoteWindowTitleBar() {
      InitializeComponent();
    }

    // @TODO: Use command instead and let the note window to handle it.
    private void OnToggleTopmost(object sender, RoutedEventArgs e) {
      var window = Window.GetWindow(this);
      window.Topmost = !window.Topmost;
    }

    private void OnClose(object sender, RoutedEventArgs args) {
      Window.GetWindow(this).Close();
    }

    // @TODO: Use command.
    private void OnMenu(object sender, RoutedEventArgs args) {
      var window = (NoteWindow)Window.GetWindow(this);
      window.ShowOverlay();
    }

    private void OnNewNote(object sender, RoutedEventArgs args) {
      var window = App.Current.MainWindow;
      if (window == null) return;

      window.OnAddNote();
    }

    private void OnMaximizeOrRestore(object sender, RoutedEventArgs args) {
      var state = Window.GetWindow(this).WindowState;
      var newState = state == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
      Window.GetWindow(this).WindowState = newState;
    }

    private void OnMouseDown(object sender, MouseButtonEventArgs args) {
      var doubleClick = args.ClickCount >= 2;
      if (!doubleClick) return;

      OnMaximizeOrRestore(sender, args);
    }

    private void OnMouseMove(object sender, MouseEventArgs args) {
      if (args.LeftButton != MouseButtonState.Pressed) return;

      var window = Window.GetWindow(this);

      if (window.WindowState == System.Windows.WindowState.Maximized) {
        var pointScreenSpace = window.PointToScreen(args.GetPosition(window));

        window.WindowState = System.Windows.WindowState.Normal;
        var halfWidthAfter = 0.5f * window.Width;

        window.Left = pointScreenSpace.X - halfWidthAfter;
        window.Top = 0;
      }

      window.DragMove();
    }
  }

}
