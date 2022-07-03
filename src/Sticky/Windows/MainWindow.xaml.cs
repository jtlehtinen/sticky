using System.Windows;
using System.Windows.Controls;
using Sticky.ViewModels;
using Sticky.DataAccess;

namespace Sticky {

  public enum PageType {
    Main,
    Settings,
  }

  public partial class MainWindow : Window {
    private UserControl _mainPage;
    private UserControl _settingsPage;

    public MainWindow(Database db) {
      InitializeComponent();
      Native.ApplyRoundedWindowCorners(this);

      _mainPage = new MainPage(new MainPageViewModel(db));
      _settingsPage = new SettingsPage(new SettingsPageViewModel(db.GetSettings(), db));

      Navigate(PageType.Main);
    }

    public void Navigate(PageType type) {
      var page = (type == PageType.Main ? _mainPage : _settingsPage);
      Content = page;
    }
  }

}
