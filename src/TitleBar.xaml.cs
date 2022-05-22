using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sticky {
  /// <summary>
  /// Interaction logic for TitleBar.xaml
  /// </summary>
  public partial class TitleBar : UserControl {

    public TitleBar() {
      InitializeComponent();
    }

    private void OnAdd(object sender, RoutedEventArgs args) {
      System.Console.WriteLine("OnAdd()");
    }

    private void OnSettings(object sender, RoutedEventArgs args) {
      System.Console.WriteLine("OnSettings()");
    }

    private void OnClose(object sender, RoutedEventArgs args) {
      Window.GetWindow(this).Close();
    }

    private void OnMouseDown(object sender, MouseButtonEventArgs args) { }

    private void OnMouseMove(object sender, MouseEventArgs args) {
      if (args.LeftButton != MouseButtonState.Pressed) return;

      var window = Window.GetWindow(this);

      if (window.WindowState == System.Windows.WindowState.Maximized) {
        var pointScreenSpace = window.PointToScreen(args.GetPosition(window));

        window.WindowState = System.Windows.WindowState.Normal;
        var halfWidthAfter = 0.5f * window.Width;

        window.Left = pointScreenSpace.X - halfWidthAfter;
        window.Top = 0;
      }

      window.DragMove();
    }
  }
}
