using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sticky {
  /// <summary>
  /// Interaction logic for TitleBar.xaml
  /// </summary>
  public partial class TitleBar : UserControl {

    public TitleBar() {
      InitializeComponent();
    }

    private void OnAdd(object sender, RoutedEventArgs args) { }

    private void OnSettings(object sender, RoutedEventArgs args) { }

    private void OnClose(object sender, RoutedEventArgs args) {
      Window.GetWindow(this).Close();
    }

    private void OnMaximizeOrRestore(object sender, RoutedEventArgs args) {
      var state = Window.GetWindow(this).WindowState;
      var newState = state == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
      Window.GetWindow(this).WindowState = newState;

      OnWindowStateChanged();
    }

    private void OnMinimize(object sender, RoutedEventArgs args) {
      Window.GetWindow(this).WindowState = WindowState.Minimized;
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

        OnWindowStateChanged();
      }

      window.DragMove();
    }

    private void OnWindowStateChanged() {
      const string ChromeMaximize = "\uE922";
      const string ChromeRestore = "\uE923";

      var state = Window.GetWindow(this).WindowState;
      if (state == WindowState.Maximized) ButtonMaximizeOrRestore.Content = ChromeRestore;
      if (state == WindowState.Normal) ButtonMaximizeOrRestore.Content = ChromeMaximize;
    }
  }
}
