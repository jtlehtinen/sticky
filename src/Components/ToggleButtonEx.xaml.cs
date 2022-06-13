using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Sticky {

  /// <summary>
  /// ToggleButtonEx is a ToggleButton that has properties for
  /// hover and pressed colors.
  /// </summary>
  public partial class ToggleButtonEx : ToggleButton {
    public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(SolidColorBrush), typeof(ToggleButtonEx), new PropertyMetadata(new SolidColorBrush(Colors.Transparent), OnBackgroundColorChanged));
    public static readonly DependencyProperty HoverBackgroundColorProperty = DependencyProperty.Register("HoverBackgroundColor", typeof(SolidColorBrush), typeof(ToggleButtonEx));
    public static readonly DependencyProperty PressedBackgroundColorProperty = DependencyProperty.Register("PressedBackgroundColor", typeof(SolidColorBrush), typeof(ToggleButtonEx));

    private static void OnBackgroundColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
      var button = obj as ToggleButtonEx;
      if (button == null) return;

      var brush = (SolidColorBrush)e.NewValue;
      var isChecked = button.IsChecked ?? false;
      button.Background = isChecked ? new SolidColorBrush(brush.Color.ToHoverColor()) : brush;
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
