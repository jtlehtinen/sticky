using System.Windows;

namespace Sticky {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {

    public MainWindow() {
      DataContext = App.Current.ViewModel;

      InitializeComponent();
      Native.ApplyRoundedWindowCorners(this);
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e) {
      // @NOTE: Hack! When the window is maximized the window size ends
      // up being greater than the monitor size.
      var thickness = (this.WindowState == WindowState.Maximized ? 8 : 0);
      this.BorderThickness = new System.Windows.Thickness(thickness);
    }
  }
}
