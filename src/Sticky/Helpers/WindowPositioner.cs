using System;
using System.Collections.Generic;
using System.Windows;

namespace Sticky.Helpers {

  public enum Anchor {
    TopLeft, TopMiddle, TopRight,
    BottomLeft, BottomMiddle, BottomRight,
    LeftBottom, LeftMiddle, LeftTop,
    RightBottom, RightMiddle, RightTop,

    Center
  }

  public static class WindowPositioner {
    private static Random _random = new Random();

    private static double RandomBetween(double min, double max) {
      return min + _random.NextDouble() * (max - min);
    }

    private static Point AnchorOffset(Anchor anchor, double width, double height) {
      switch (anchor) {
        case Anchor.TopLeft: return new Point(0, 0);
        case Anchor.TopMiddle: return new Point(-width / 2, 0);
        case Anchor.TopRight: return new Point(-width, 0);
        case Anchor.BottomLeft: return new Point(0, -height);
        case Anchor.BottomMiddle: return new Point(-width / 2, -height);
        case Anchor.BottomRight: return new Point(-width, -height);
        case Anchor.LeftBottom: return new Point(0, -height);
        case Anchor.LeftMiddle: return new Point(0, -height / 2);
        case Anchor.LeftTop: return new Point(0, 0);
        case Anchor.RightBottom: return new Point(-width, -height);
        case Anchor.RightMiddle: return new Point(-width, -height / 2);
        case Anchor.RightTop: return new Point(-width, 0);
        case Anchor.Center: return new Point(-width / 2, -height / 2);
        default: return new Point(0, 0);
      }
    }

    private static Point AnchorOffsetReference(Anchor anchor, double width, double height, double distance = 0) {
      switch (anchor) {
        case Anchor.TopLeft: return new Point(0, -distance);
        case Anchor.TopMiddle: return new Point(width / 2, -distance);
        case Anchor.TopRight: return new Point(width, -distance);
        case Anchor.BottomLeft: return new Point(0, height + distance);
        case Anchor.BottomMiddle: return new Point(width / 2, height + distance);
        case Anchor.BottomRight: return new Point(width, height + distance);
        case Anchor.LeftBottom: return new Point(-distance, height);
        case Anchor.LeftMiddle: return new Point(-distance, height / 2);
        case Anchor.LeftTop: return new Point(-distance, 0);
        case Anchor.RightBottom: return new Point(width + distance, height);
        case Anchor.RightMiddle: return new Point(width + distance, height / 2);
        case Anchor.RightTop: return new Point(width + distance, 0);
        case Anchor.Center: return new Point(width / 2, height / 2);
        default: return new Point(0, 0);
      }
    }

    private static bool EpsilonEquals(double a, double b, double epsilon) {
      return Math.Abs(a - b) <= epsilon;
    }

    private static bool PositionTaken(WindowCollection windows, Point position) {
      const double epsilon = 5.0;

      for (var i = 0; i < windows.Count; ++i) {
        var window = windows[i];
        if (EpsilonEquals(window.Left, position.X, epsilon) && EpsilonEquals(window.Top, position.Y, epsilon)) {
          return true;
        }
      }
      return false;
    }

    private static Point Position(Window relativeTo, Window window, Anchor relativeToAnchor, Anchor windowAnchor) {
      var left = relativeTo.Left;
      var top = relativeTo.Top;

      var referenceOffset = AnchorOffsetReference(relativeToAnchor, relativeTo.Width, relativeTo.Height, 10.0);
      var windowOffset = AnchorOffset(windowAnchor, window.Width, window.Height);

      var position = new Point(
        left + referenceOffset.X + windowOffset.X,
        top + referenceOffset.Y + windowOffset.Y
      );

      return position;
    }

    private static bool IsOnScreen(IEnumerable<WpfScreenHelper.Screen> screens, Rect rect) {
      foreach (var screen in screens) {
        if (screen.Bounds.Contains(rect.BottomLeft) &&
          screen.Bounds.Contains(rect.BottomRight) &&
          screen.Bounds.Contains(rect.TopLeft) &&
          screen.Bounds.Contains(rect.TopRight)) {
          return true;
        }
      }

      return false;
    }

    public static void Position(WindowCollection avoidWindows, Window relativeTo, Window window) {
      var screens = WpfScreenHelper.Screen.AllScreens;
      var windowSize = new Size(window.Width, window.Height);

      var position = Position(relativeTo, window, Anchor.RightTop, Anchor.TopLeft);

      if (!PositionTaken(avoidWindows, position) && IsOnScreen(screens, new Rect(position, windowSize))) {
        window.Left = position.X;
        window.Top = position.Y;
        return;
      }

      position = Position(relativeTo, window, Anchor.LeftTop, Anchor.TopRight);

      if (!PositionTaken(avoidWindows, position) && IsOnScreen(screens, new Rect(position, windowSize))) {
        window.Left = position.X;
        window.Top = position.Y;
        return;
      }

      position = Position(relativeTo, window, Anchor.Center, Anchor.Center);

      position.X += RandomBetween(-100, 100);
      position.Y += RandomBetween(-100, 100);

      window.Left = position.X;
      window.Top = position.Y;
    }

  }

}
