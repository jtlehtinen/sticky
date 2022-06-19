using System.Windows;

namespace Sticky {

  public partial class MainWindowTitleBar : TitleBarBase {
    public MainWindowTitleBar() {
      InitializeComponent();
    }

    private void OnSettings(object sender, RoutedEventArgs args) {
      System.Console.WriteLine("OnSettings()");
    }
  }

}
