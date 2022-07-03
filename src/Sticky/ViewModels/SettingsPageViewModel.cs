using System.Windows;
using System.Windows.Input;
using Sticky.DataAccess;

namespace Sticky.ViewModels {

  public class SettingsPageViewModel : ViewModelBase {
    public ICommand BackCommand { get; }
    public ICommand CloseCommand { get; }
    public ICommand ChangeBaseThemeCommand { get; }

    private Settings _settings;
    private Database _db;

    public SettingsPageViewModel(Settings settings, Database db) {
      this._settings = settings;
      this._db = db;

      BackCommand = new RelayCommand(() => App.Current.MainWindow.Navigate(PageType.Main));
      CloseCommand = new RelayCommand(() => App.Current.MainWindow.Close());
      ChangeBaseThemeCommand = new RelayCommand((param) => BaseTheme = (BaseTheme)param);

      PropertyChanged += (sender, e) => _db.UpdateSettings(_settings);

      _db.SettingsModified += (sender, e) => {
        if (ConfirmBeforeDelete != e.NewSettings.ConfirmBeforeDelete) ConfirmBeforeDelete = e.NewSettings.ConfirmBeforeDelete;
        if (PinNewNote != e.NewSettings.PinNewNote) PinNewNote = e.NewSettings.PinNewNote;
        if (BaseTheme != e.NewSettings.BaseTheme) BaseTheme = e.NewSettings.BaseTheme;
      };
    }

    public bool ConfirmBeforeDelete {
      get { return _settings.ConfirmBeforeDelete; }
      set { _settings.ConfirmBeforeDelete = value; OnPropertyChanged(); }
    }

    public bool PinNewNote {
      get { return _settings.PinNewNote; }
      set { _settings.PinNewNote = value; OnPropertyChanged(); }
    }

    public BaseTheme BaseTheme {
      get { return _settings.BaseTheme; }
      set { _settings.BaseTheme = value; OnPropertyChanged(); }
    }
  }

}
