using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace ModernWpf.Controls.Primitives {
  public class RelativeToAbsoluteConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      if (value != null) {
        if (parameter is string) {
          return new Uri(Path.Combine((string)parameter, value.ToString()));
        } else {
          return new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, value.ToString().Replace("/", @"\")));
        }
      }
      return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
