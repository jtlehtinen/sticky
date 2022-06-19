using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sticky {

  public abstract class TitleBarBase : UserControl {
    protected void OnClose(object sender, RoutedEventArgs args) {
      Window.GetWindow(this).Close();
    }

    protected void OnMaximizeOrRestore(object sender, RoutedEventArgs args) {
      var state = Window.GetWindow(this).WindowState;
      var newState = state == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
      Window.GetWindow(this).WindowState = newState;
    }

    protected void OnMouseDown(object sender, MouseButtonEventArgs args) {
      var doubleClick = args.ClickCount >= 2;
      if (!doubleClick) return;

      OnMaximizeOrRestore(sender, args);
    }

    protected void OnMouseMove(object sender, MouseEventArgs args) {
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

