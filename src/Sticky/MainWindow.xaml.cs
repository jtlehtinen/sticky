using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using ModernWpf.Controls;

namespace Sticky {

  public enum PageType {
    Main,
    Settings,
  }

  public partial class MainWindow : Window {
    private UserControl mainPage;
    private UserControl settingsPage;

    public MainWindow() {
      DataContext = App.Current.ViewModel;

      InitializeComponent();
      Native.ApplyRoundedWindowCorners(this);

      mainPage = new MainPage();
      settingsPage = new SettingsPage();

      Navigate(PageType.Main);
    }

    public void Navigate(PageType type) {
      if (type == PageType.Main) {
        Content = mainPage;
      } else if (type == PageType.Settings) {
        Content = settingsPage;
      }
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e) {
      // @HACK: When the window is maximized the window size ends
      // up being greater than the monitor size.
      var thickness = (this.WindowState == WindowState.Maximized ? 8 : 0);
      this.BorderThickness = new System.Windows.Thickness(thickness);
    }
  }

}
