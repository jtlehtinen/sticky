using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Sticky {

  public static class Animation {

    public static TranslateTransform MakeTranslateYTransform(double from, double to, int millis) {
      var transform = new TranslateTransform(0, 0);
      var duration = new Duration(new TimeSpan(0, 0, 0, 0, millis));
      var animation = new DoubleAnimation(from, to, duration);
      transform.BeginAnimation(TranslateTransform.YProperty, animation);
      return transform;
    }

  }
}
