using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media ;
using Sticky.DataAccess;

namespace Sticky.ViewModels {

  public record ColorTheme(string Name, SolidColorBrush AccentColor, bool Active) { }

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
      _colorThemes = GetColorThemes(note.Theme);

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
      get {
        // @TODO: ...
        const string EMPTY_NOTE = "\u003CSection xmlns=\u0022http://schemas.microsoft.com/winfx/2006/xaml/presentation\u0022 xml:space=\u0022preserve\u0022 TextAlignment=\u0022Left\u0022 LineHeight=\u0022Auto\u0022 IsHyphenationEnabled=\u0022False\u0022 xml:lang=\u0022en-us\u0022 FlowDirection=\u0022LeftToRight\u0022 NumberSubstitution.CultureSource=\u0022Text\u0022 NumberSubstitution.Substitution=\u0022AsCulture\u0022 FontFamily=\u0022Segoe UI\u0022 FontStyle=\u0022Normal\u0022 FontWeight=\u0022Normal\u0022 FontStretch=\u0022Normal\u0022 FontSize=\u002214\u0022 Foreground=\u0022#FF000000\u0022 Typography.StandardLigatures=\u0022True\u0022 Typography.ContextualLigatures=\u0022True\u0022 Typography.DiscretionaryLigatures=\u0022False\u0022 Typography.HistoricalLigatures=\u0022False\u0022 Typography.AnnotationAlternates=\u00220\u0022 Typography.ContextualAlternates=\u0022True\u0022 Typography.HistoricalForms=\u0022False\u0022 Typography.Kerning=\u0022True\u0022 Typography.CapitalSpacing=\u0022False\u0022 Typography.CaseSensitiveForms=\u0022False\u0022 Typography.StylisticSet1=\u0022False\u0022 Typography.StylisticSet2=\u0022False\u0022 Typography.StylisticSet3=\u0022False\u0022 Typography.StylisticSet4=\u0022False\u0022 Typography.StylisticSet5=\u0022False\u0022 Typography.StylisticSet6=\u0022False\u0022 Typography.StylisticSet7=\u0022False\u0022 Typography.StylisticSet8=\u0022False\u0022 Typography.StylisticSet9=\u0022False\u0022 Typography.StylisticSet10=\u0022False\u0022 Typography.StylisticSet11=\u0022False\u0022 Typography.StylisticSet12=\u0022False\u0022 Typography.StylisticSet13=\u0022False\u0022 Typography.StylisticSet14=\u0022False\u0022 Typography.StylisticSet15=\u0022False\u0022 Typography.StylisticSet16=\u0022False\u0022 Typography.StylisticSet17=\u0022False\u0022 Typography.StylisticSet18=\u0022False\u0022 Typography.StylisticSet19=\u0022False\u0022 Typography.StylisticSet20=\u0022False\u0022 Typography.Fraction=\u0022Normal\u0022 Typography.SlashedZero=\u0022False\u0022 Typography.MathematicalGreek=\u0022False\u0022 Typography.EastAsianExpertForms=\u0022False\u0022 Typography.Variants=\u0022Normal\u0022 Typography.Capitals=\u0022Normal\u0022 Typography.NumeralStyle=\u0022Normal\u0022 Typography.NumeralAlignment=\u0022Normal\u0022 Typography.EastAsianWidths=\u0022Normal\u0022 Typography.EastAsianLanguage=\u0022Normal\u0022 Typography.StandardSwashes=\u00220\u0022 Typography.ContextualSwashes=\u00220\u0022 Typography.StylisticAlternates=\u00220\u0022\u003E\u003CParagraph\u003E\u003CRun\u003E\u003C/Run\u003E\u003C/Paragraph\u003E\u003C/Section\u003E";
        return string.IsNullOrWhiteSpace(Content) || Content == EMPTY_NOTE;
      }
    }
  }

}
