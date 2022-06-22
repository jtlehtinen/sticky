using System.Windows;

namespace Sticky {

  public partial class MainPageTitleBar : TitleBarBase {
    public MainPageTitleBar() {
      InitializeComponent();
    }

    private void OnSettings(object sender, RoutedEventArgs args) {
      App.Current.MainWindow?.Navigate(PageType.Settings);
    }
  }

}
