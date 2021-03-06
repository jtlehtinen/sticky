using System;
using System.Windows;
using System.Windows.Input;

namespace Sticky {

  public class DragBehavior {
    private UIElement _element;
    private System.Windows.Point _startPoint;

    public DragBehavior(UIElement element) {
      _element = element;
      element.MouseLeftButtonDown += OnMouseLeftButtonDown;
      element.MouseMove += OnMouseMove;
    }

    private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
      var doubleClick = e.ClickCount >= 2;
      if (doubleClick) {
        MaximizeOrRestore();
      } else {
        var window = Window.GetWindow(_element);
        _startPoint = window.PointToScreen(e.GetPosition(window));
      }
    }

    private void OnMouseMove(object sender, MouseEventArgs e) {
      if (e.LeftButton != MouseButtonState.Pressed) return;

      var window = Window.GetWindow(_element);
      var point = window.PointToScreen(e.GetPosition(window));
      var dx = Math.Abs(point.X - _startPoint.X);
      var dy = Math.Abs(point.Y - _startPoint.Y);

      if (Math.Abs(dx) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(dy) > SystemParameters.MinimumVerticalDragDistance) {
        StartDrag(e);
      }
    }

    private void MaximizeOrRestore() {
      var window = Window.GetWindow(_element);

      var newState = window.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
      window.WindowState = newState;
    }

    private void StartDrag(MouseEventArgs e) {
      var window = Window.GetWindow(_element);

      if (window.WindowState == WindowState.Maximized) {
        var pointScreenSpace = window.PointToScreen(e.GetPosition(window));

        window.WindowState = WindowState.Normal;
        var halfWidthAfter = 0.5f * window.Width;

        window.Left = pointScreenSpace.X - halfWidthAfter;
        window.Top = 0;
      }

      window.DragMove();
    }

  }

  public class FixMaximizedWindowSizeBehavior {
    private Window _window;

    public FixMaximizedWindowSizeBehavior(Window window) {
      this._window = window;
      _window.SizeChanged += OnSizeChanged;
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e) {
      // @NOTE: Hack! When the window is maximized the window size ends
      // up being greater than the monitor size.
      var thickness = (_window.WindowState == WindowState.Maximized ? 8 : 0);
      _window.BorderThickness = new System.Windows.Thickness(thickness);
    }
  }

}
