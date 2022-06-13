using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sticky {

  // https://stackoverflow.com/questions/20073294/change-color-of-button-when-mouse-is-over

  /// <summary>
  /// ButtonEx is a button that has properties for
  /// hover and pressed colors.
  /// </summary>
  public partial class ButtonEx : Button {
    public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(SolidColorBrush), typeof(ButtonEx), new PropertyMetadata(new SolidColorBrush(Colors.Transparent), OnBackgroundColorChanged));
    public static readonly DependencyProperty HoverBackgroundColorProperty = DependencyProperty.Register("HoverBackgroundColor", typeof(SolidColorBrush), typeof(ButtonEx));
    public static readonly DependencyProperty PressedBackgroundColorProperty = DependencyProperty.Register("PressedBackgroundColor", typeof(SolidColorBrush), typeof(ButtonEx));
    public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(ButtonEx), new PropertyMetadata(false, OnIsActiveChanged));

    private static void OnBackgroundColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
      var button = obj as ButtonEx;
      if (button == null) return;

      var brush = (SolidColorBrush)e.NewValue;

      var active = button.IsActive;
      button.Background = active ? new SolidColorBrush(brush.Color.ToHoverColor()) : brush;
      button.HoverBackgroundColor = new SolidColorBrush(brush.Color.ToHoverColor());
      button.PressedBackgroundColor = new SolidColorBrush(brush.Color.ToPressedColor());
    }

    private static void OnIsActiveChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
      var button = obj as ButtonEx;
      if (button == null) return;

      var active = button.IsActive;
      var background = button.BackgroundColor;
      button.Background = active ? new SolidColorBrush(background.Color.ToHoverColor()) : background;
    }

    public ButtonEx() {
      //InitializeComponent();
    }

    public bool IsActive {
      get { return (bool)GetValue(IsActiveProperty); }
      set { SetValue(IsActiveProperty, value); }
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
