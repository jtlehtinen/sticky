using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sticky {

  public partial class RadioButtonEx : RadioButton {
    public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(SolidColorBrush), typeof(RadioButtonEx), new PropertyMetadata(new SolidColorBrush(Colors.Transparent), OnBackgroundColorChanged));
    public static readonly DependencyProperty HoverBackgroundColorProperty = DependencyProperty.Register("HoverBackgroundColor", typeof(SolidColorBrush), typeof(RadioButtonEx));
    public static readonly DependencyProperty PressedBackgroundColorProperty = DependencyProperty.Register("PressedBackgroundColor", typeof(SolidColorBrush), typeof(RadioButtonEx));

    private static void OnBackgroundColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
      var button = obj as RadioButtonEx;
      if (button == null) return;

      var brush = (SolidColorBrush)e.NewValue;
      button.Background = brush;
      button.HoverBackgroundColor = new SolidColorBrush(brush.Color.ToHoverColor());
      button.PressedBackgroundColor = new SolidColorBrush(brush.Color.ToPressedColor());
    }

    public SolidColorBrush BackgroundColor {
      get { return (SolidColorBrush)GetValue(BackgroundColorProperty); }
      set { SetValue(BackgroundColorProperty, value); }
    }

    public SolidColorBrush HoverBackgroundColor {
      get { return (SolidColorBrush)GetValue(HoverBackgroundColorProperty); }
      set { SetValue(HoverBackgroundColorProperty, value); }
    }

    public SolidColorBrush PressedBackgroundColor {
      get { return (SolidColorBrush)GetValue(PressedBackgroundColorProperty); }
      set { SetValue(PressedBackgroundColorProperty, value); }
    }
  }

}
