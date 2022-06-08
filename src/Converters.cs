using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace Sticky {

  // @TODO: Move to some other file...
  // @NOTE: Only reason this exists to be able to use value
  // converters with dynamic resources.
  //
  // https://thomaslevesque.com/2011/03/21/wpf-how-to-bind-to-data-when-the-datacontext-is-not-inherited/
  public class BindingProxy : Freezable {
    protected override Freezable CreateInstanceCore() {
      return new BindingProxy();
    }

    public object Data {
      get { return (object)GetValue(DataProperty); }
      set { SetValue(DataProperty, value); }
    }

    public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));
  }

  public class StringToFlowDocumentConverter : IValueConverter {
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      if (value == null) return null;

      var s = (string)value;

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

  public class BackgroundToHoverConverter : IValueConverter {
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      if (value == null) return null;

      var brush = (SolidColorBrush)value;
      return new SolidColorBrush(brush.Color.ToHoverColor());
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }

  public class BackgroundToPressedConverter : IValueConverter {
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
