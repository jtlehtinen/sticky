using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Sticky {

  public class FoldedCornerTriangleConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      var dim = (double)value;

      var collection = new PointCollection();
      collection.Add(new Point(0, dim));
      collection.Add(new Point(dim, dim));
      collection.Add(new Point(dim, 0));

      return collection;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }

  public class FoldedCornerConverter : IMultiValueConverter {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
      var width = (double)values[0];
      var height = (double)values[1];
      var open = (bool)values[2];

      if (!open) {
        return new RectangleGeometry(new Rect(0, 0, width, height));
      }

      // Cut a triangle from top-left corner.
      var dim = (double)parameter;
      var start = new Point(dim, 0);

      var segments = new List<LineSegment>{
        new LineSegment(new Point(width,      0), false),
        new LineSegment(new Point(width, height), false),
        new LineSegment(new Point(    0, height), false),
        new LineSegment(new Point(    0,    dim), false),
      };

      var figure = new PathFigure(start, segments, true);

      return new PathGeometry(new List<PathFigure>{ figure });
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }

  public class BooleanToGeometryCombineModeConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      var bvalue = (bool)value;
      return (bool)value ? GeometryCombineMode.Union : GeometryCombineMode.Exclude;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }

  public class ReverseBooleanToGeometryCombineModeConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      var bvalue = (bool)value;
      return (bool)value ? GeometryCombineMode.Exclude : GeometryCombineMode.Union;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }

  public class BaseThemeToIndexConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      var theme = (BaseTheme)value;
      if (theme == BaseTheme.Light) return 0;
      if (theme == BaseTheme.Dark) return 1;
      if (theme == BaseTheme.System) return 2;

      throw new ArgumentException("value");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      var index = (int)value;
      if (index == 0) return BaseTheme.Light;
      if (index == 1) return BaseTheme.Dark;
      if (index == 2) return BaseTheme.System;

      throw new ArgumentException("value");
    }
  }

  public class StringToThemeConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      var themeName = (string)value;
      var theme = App.Current.Themes.GetTheme(themeName);
      return theme;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      var dic = (ResourceDictionary)value;
      if (!App.Current.Themes.IsTheme(dic)) return null;

      return App.Current.Themes.GetThemeName(dic);
    }
  }

  public class ReverseBooleanToVisibilityMultiConverter : IMultiValueConverter {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
      foreach (var value in values) {
        if ((bool)value) return Visibility.Collapsed;
      }
      return Visibility.Visible;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }

  public class BooleanToVisibilityMultiConverter : IMultiValueConverter {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
      foreach (var value in values) {
        if ((bool)value) return Visibility.Visible;
      }
      return Visibility.Collapsed;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }

  public class ReverseBooleanToVisibilityConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      var flag = (bool)value;
      return flag ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      var visibility = (Visibility)value;
      return visibility == Visibility.Collapsed;
    }
  }

  public class BooleanToVisibilityHiddenConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      var flag = (bool)value;
      return flag ? Visibility.Visible : Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      var visibility = (Visibility)value;
      return visibility == Visibility.Visible;
    }
  }

  public class StringToFlowDocumentConverter : IValueConverter {
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      var s = value as string;
      if (s == null) return null;

      var result = XamlReader.Parse(s);
      return result;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }

  public class DateTimeConverter : IValueConverter {
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      if (value == null) return null;

      var dt = (DateTime)value;
      var today = DateTime.Today;

      // https://stackoverflow.com/questions/3025361/c-sharp-datetime-to-yyyymmddhhmmss-format

      var isToday = (dt.Year == today.Year && dt.Month == today.Month && dt.Day == today.Day);
      var fmt = isToday ? "{0:t}" : "{0:m}";

      return String.Format(fmt, dt);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }

  public class ColorToInactiveConverter : IValueConverter {
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      if (value == null) return null;

      var brush = (SolidColorBrush)value;
      return new SolidColorBrush(brush.Color.ToInactiveColor());
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }

  public class ColorToHoverConverter : IValueConverter {
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      if (value == null) return null;

      var brush = (SolidColorBrush)value;
      return new SolidColorBrush(brush.Color.ToHoverColor());
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }

  public class ColorToPressedConverter : IValueConverter {
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      if (value == null) return null;

      var brush = (SolidColorBrush)value;
      return new SolidColorBrush(brush.Color.ToPressedColor());
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }

}
