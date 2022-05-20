using System.Windows;
using System.Windows.Input;

namespace Sticky {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs args) {
      // @NOTE: This hack is here because:
      // Given: AllowsTransparency="True"
      // When: Window maximized
      // Then: Window size is greater than the monitor size... WTF?

      var thickness = (this.WindowState == WindowState.Maximized ? 8 : 0);
      this.BorderThickness = new System.Windows.Thickness(thickness);
    }
  }
}

