using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media ;
using Sticky.DataAccess;

namespace Sticky.ViewModels {

  public class ColorTheme : ViewModelBase {
    private string _name;
    private SolidColorBrush _accentColor;
    private bool _active;

    public ColorTheme(string name, SolidColorBrush accentColor, bool active) {
      _name = name;
      _accentColor = accentColor;
      _active = active;
    }

    public string Name {
      get => _name;
      set => SetProperty(ref _name, value);
    }

    public SolidColorBrush AccentColor {
      get => _accentColor;
      set => SetProperty(ref _accentColor, value);
    }

    public bool Active {
      get => _active;
      set => SetProperty(ref _active, value);
    }
  }

  public class NoteWindowViewModel : ViewModelBase {
    public ICommand PinCommand { get; }
    public ICommand UnpinCommand { get; }
    public ICommand CloseCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand NewNoteCommand { get; }
    public ICommand ShowOverlayCommand { get; }
    public ICommand HideOverlayCommand { get; }
    public ICommand ChangeNoteThemeCommand { get; }
    public ICommand OpenNoteListCommand { get; }
    public ICommand ActivatePreviousWindowCommand { get; }
    public ICommand ActivateNextWindowCommand { get; }

    public event Action ShowOverlayRequested;
    public event Action HideOverlayRequested;
    public event Action OpenNoteListRequested;
    public event Action<Window> ActivatePreviousWindowRequested;
    public event Action<Window> ActivateNextWindowRequested;

    private Note _note;
    private Database _db;
    private List<ColorTheme> _colorThemes;

    public NoteWindowViewModel(Note note, Database db) {
      this._note = note;
      this._db = db;
      _colorThemes = GetColorThemes(_note.Theme);

      App.Current.Themes.BaseThemeChanged -= OnBaseThemeChanged;
      App.Current.Themes.BaseThemeChanged += OnBaseThemeChanged;

      ActivatePreviousWindowCommand = new RelayCommand((param) => ActivatePreviousWindowRequested?.Invoke((Window)param));
      ActivateNextWindowCommand = new RelayCommand((param) => ActivateNextWindowRequested?.Invoke((Window)param));

      PinCommand = new RelayCommand(() => IsAlwaysOnTop = true);
      UnpinCommand = new RelayCommand(() => IsAlwaysOnTop = false);

      CloseCommand = new RelayCommand(() => {
        // @NOTE: When an empty note is closed it is removed.
        if (IsEmpty) {
          _db.DeleteNote(_note);
        } else {
          IsOpen = false;
        }
      });

      DeleteCommand = new RelayCommand(async () => {
        var settings = _db.GetSettings();
        if (!settings.ConfirmBeforeDelete) {
          db.DeleteNote(_note);
          return;
        }

        var result = await Dialogs.ConfirmDelete();

        if (result.DontAskAgain) {
          settings.ConfirmBeforeDelete = false;
          _db.UpdateSettings(settings);
        }

        if (result.DoDelete) {
          db.DeleteNote(_note);
        }
      });
      NewNoteCommand = new RelayCommand(() => _db.AddNote(new Note())); // @TODO: Apply settings...
      ShowOverlayCommand = new RelayCommand(() => ShowOverlayRequested?.Invoke());
      HideOverlayCommand = new RelayCommand(() => HideOverlayRequested?.Invoke());
      ChangeNoteThemeCommand = new RelayCommand((param) => Theme = (string)param);
      OpenNoteListCommand = new RelayCommand(() => OpenNoteListRequested?.Invoke());

      PropertyChanged += (sender, e) => {
        _db.UpdateNote(_note, e.PropertyName != "WindowPosition"); // @TODO: ...
      };
    }

    private void OnBaseThemeChanged() {
      ColorThemes = GetColorThemes(_note.Theme);
      OnPropertyChanged(nameof(ColorThemes));
      OnPropertyChanged(nameof(IsDarkTheme));
    }

    private List<ColorTheme> GetColorThemes(string activeThemeName) {
      var result = new List<ColorTheme>();

      var themes = App.Current.Themes.GetThemes();
      foreach (var key in themes.Keys) {
        var theme = themes[key];
        var name = App.Current.Themes.GetThemeName(theme);
        var active = activeThemeName == name;

        result.Add(new ColorTheme(name, (SolidColorBrush)theme["AccentColor"], active));
      }

      return result;
    }

    public bool IsDarkTheme {
      get { return App.Current.Themes.UseDarkTheme(); }
    }

    public List<ColorTheme> ColorThemes {
      get { return _colorThemes; }
      set { _colorThemes = value; OnPropertyChanged(); }
    }

    public int Id { get { return _note.Id; } }

    public DateTime CreatedAt {
      get { return _note.CreatedAt; }
    }

    public string Content {
      get { return _note.Content; }
      set { _note.Content = value; OnPropertyChanged(); }
    }

    public string Theme {
      get { return _note.Theme; }
      set { _note.Theme = value; OnPropertyChanged(); }
    }

    public bool IsOpen {
      get { return _note.IsOpen; }
      set { _note.IsOpen = value; OnPropertyChanged(); }
    }

    public bool IsAlwaysOnTop {
      get { return _note.IsAlwaysOnTop; }
      set { _note.IsAlwaysOnTop = value; OnPropertyChanged(); }
    }

    public string WindowPosition {
      get { return _note.WindowPosition; }
      set { _note.WindowPosition = value; OnPropertyChanged(); }
    }

    public bool IsEmpty {
      get { return _note.IsEmpty(); }
    }
  }

}
