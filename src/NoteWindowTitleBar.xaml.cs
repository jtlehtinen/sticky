using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sticky {
  /// <summary>
  /// Interaction logic for TitleBar.xaml
  /// </summary>
  public partial class NoteWindowTitleBar : UserControl {
    public NoteWindowTitleBar() {
      InitializeComponent();
    }

    // @TODO: Pass the events to the main window...
    // @TODO: Reuse title bar?

    private void OnAdd(object sender, RoutedEventArgs args) {
      // @NOTE: Would routed events be better here?
      var window = Application.Current.MainWindow as MainWindow;
      if (window == null) return;

      window.OnAddNote();
    }

    private void OnMenu(object sender, RoutedEventArgs args) {
      System.Console.WriteLine("OnSettings()");
    }

    private void OnClose(object sender, RoutedEventArgs args) {
      Window.GetWindow(this).Close();
    }

    private void OnMouseDown(object sender, MouseButtonEventArgs args) {
      var doubleClick = args.ClickCount >= 2;
      if (!doubleClick) return;

      OnMaximizeOrRestore(sender, args);
    }

    private void OnMaximizeOrRestore(object sender, RoutedEventArgs args) {
      var state = Window.GetWindow(this).WindowState;
      var newState = state == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
      Window.GetWindow(this).WindowState = newState;
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
