using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Sticky {
  /// <summary>
  /// Interaction logic for NoteWindow.xaml
  /// </summary>
  public partial class NoteWindow : Window {

    public NoteWindow() {
      InitializeComponent();
      Native.ApplyRoundedWindowCorners(this);
      this.Show();
      this.NoteInput.GotKeyboardFocus += OnGotKeyboardFocus;
      this.NoteInput.LostKeyboardFocus += OnLostKeyboardFocus;
    }

    private void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs args) {
      System.Console.WriteLine("got focus");
      var transform = new TranslateTransform(0, 0);
      var duration = new Duration(new TimeSpan(0, 0, 0, 0, 150));
      var animation = new DoubleAnimation(-42, 0, duration);
      transform.BeginAnimation(TranslateTransform.YProperty, animation);
      this.TitleBar.RenderTransform = transform;
    }

    private void OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs args) {
      System.Console.WriteLine("lost focus");
      var transform = new TranslateTransform(0, 0);
      var duration = new Duration(new TimeSpan(0, 0, 0, 0, 150));
      var animation = new DoubleAnimation(0, -42, duration);
      transform.BeginAnimation(TranslateTransform.YProperty, animation);
      this.TitleBar.RenderTransform = transform;
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs args) {
      // @NOTE: This hack is here because:
      // Given: AllowsTransparency="True"
      // When: Window maximized
      // Then: Window size is greater than the monitor size... WTF?

      var thickness = (this.WindowState == WindowState.Maximized ? 8 : 0);
      this.BorderThickness = new System.Windows.Thickness(thickness);
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs args) {
      // @TODO: Remove from main window...
      base.OnClosing(args);
    }
  }
}
