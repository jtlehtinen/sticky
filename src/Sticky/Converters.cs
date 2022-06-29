using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Sticky {

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
