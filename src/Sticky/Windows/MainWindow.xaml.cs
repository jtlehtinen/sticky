using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using Sticky.Helpers;
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
    private Database _db;

    public MainWindow(Database db) {
      this._db = db;
      this.Closing += (sender, e) => SaveWindowPlacement();

      InitializeComponent();
      Native.ApplyRoundedWindowCorners(this);

      _mainPage = new MainPage(new MainPageViewModel(db));
      _settingsPage = new SettingsPage(new SettingsPageViewModel(db.GetSettings(), db));

      LoadWindowPlacement();
      Navigate(PageType.Main);
    }

    public void Navigate(PageType type) {
      var page = (type == PageType.Main ? _mainPage : _settingsPage);
      Content = page;
    }

    private void LoadWindowPlacement() {
      var settings = _db.GetSettings();
      if (string.IsNullOrWhiteSpace(settings.MainWindowPosition)) return;

      var handle = new WindowInteropHelper(this).Handle;
      var placement = WindowHelper.DeserializePlacementOrDefault(handle, settings.MainWindowPosition);
      Native.SetWindowPlacement(handle, ref placement);
    }

    private void SaveWindowPlacement() {
      var handle = new WindowInteropHelper(this).Handle;
      var json = WindowHelper.SerializePlacement(handle);

      var settings = _db.GetSettings();
      settings.MainWindowPosition = json;
      _db.UpdateSettings(settings);
    }
  }

}
