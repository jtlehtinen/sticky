using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using Sticky.Helpers;
using Sticky.ViewModels;
using Sticky.DataAccess;

namespace Sticky {

  public enum PageType {
    Main,
    Settings,
    ThirdPartyNotices,
  }

  public partial class MainWindow : Window {
    private MainPage _mainPage;
    private SettingsPage _settingsPage;
    private ThirdPartyNoticesPage _thirdPartyNoticesPage;
    private Database _db;

    public MainWindow(Database db) {
      this._db = db;
      this.Closing += (sender, e) => SaveWindowPlacement();
      MouseDown += (sender, e) => Keyboard.ClearFocus();

      InitializeComponent();
      Native.ApplyRoundedWindowCorners(this);

      var vm = new MainWindowViewModel(db);
      vm.CloseRequested += () => Close();

      DataContext = vm;

      vm.SearchInNotesRequested += () => {
        if (Content == _mainPage) {
          _mainPage.SearchBox.Focus();
        }
      };

      vm.ActivateNextWindowRequested += (window) => WindowHelper.ActivateNextWindow(App.Current.Windows, window);
      vm.ActivatePreviousWindowRequested += (window) => WindowHelper.ActivatePreviousWindow(App.Current.Windows, window);

      _mainPage = new MainPage(new MainPageViewModel(db));
      _settingsPage = new SettingsPage(new SettingsPageViewModel(db));
      _thirdPartyNoticesPage = new ThirdPartyNoticesPage();

      LoadWindowPlacement();
      Navigate(PageType.Main);
    }

    public void Navigate(PageType type) {
      switch (type) {
        case PageType.Main: Content = _mainPage; break;
        case PageType.Settings: Content = _settingsPage; break;
        case PageType.ThirdPartyNotices: Content = _thirdPartyNoticesPage; break;
      }
    }

    private void LoadWindowPlacement() {
      var settings = _db.GetSettings();
      if (string.IsNullOrWhiteSpace(settings.MainWindowPosition)) return;

      var handle = new WindowInteropHelper(this).Handle;
      var placement = WindowHelper.DeserializePlacementOrDefault(handle, settings.MainWindowPosition);
      Native.SetWindowPlacement(handle, ref placement);
    }

    private void SaveWindowPlacement() {
      var json = WindowHelper.SerializePlacement(this);

      var settings = _db.GetSettings();
      settings.MainWindowPosition = json;
      _db.UpdateSettings(settings);
    }
  }

}
