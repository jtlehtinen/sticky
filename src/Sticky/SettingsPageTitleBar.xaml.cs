using System.Windows;

namespace Sticky {

  public partial class SettingsPageTitleBar : TitleBarBase {
    public SettingsPageTitleBar() {
      InitializeComponent();
    }

    private void OnBack(object sender, RoutedEventArgs args) {
      App.Current.MainWindow?.Navigate(PageType.Main);
    }

    private void OnSettings(object sender, RoutedEventArgs args) {
      App.Current.MainWindow?.Navigate(PageType.Settings);
    }
  }

}
