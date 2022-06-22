using System.Windows;

namespace Sticky {

  public partial class MainWindowTitleBar : TitleBarBase {
    public MainWindowTitleBar() {
      InitializeComponent();
    }

    private void OnSettings(object sender, RoutedEventArgs args) {
      App.Current.MainWindow?.Navigate(PageType.Settings);
    }
  }

}
