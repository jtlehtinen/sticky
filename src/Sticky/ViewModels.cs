using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Sticky {

  // https://docs.microsoft.com/en-us/windows/communitytoolkit/mvvm/relaycommand
  public class RelayCommand : ICommand {
    private readonly Action<object> _execute;
    private readonly Predicate<object>? _canExecute;

    public event EventHandler? CanExecuteChanged {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public RelayCommand(Action<object> execute) : this(execute, null) { }

    public RelayCommand(Action<object> execute, Predicate<object>? canExecute) {
      if (execute == null)
        throw new ArgumentNullException("execute");

      _execute = execute;
      _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter) {
      return _canExecute == null ? true : _canExecute(parameter);
    }

    public void Execute(object? parameter) {
      _execute(parameter);
    }
  }

  public class ViewModelBase : INotifyPropertyChanged {
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null) {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
  }

  public class NoteViewModel : ViewModelBase {
    // @TODO: ...
    const string EMPTY_NOTE = "\u003CSection xmlns=\u0022http://schemas.microsoft.com/winfx/2006/xaml/presentation\u0022 xml:space=\u0022preserve\u0022 TextAlignment=\u0022Left\u0022 LineHeight=\u0022Auto\u0022 IsHyphenationEnabled=\u0022False\u0022 xml:lang=\u0022en-us\u0022 FlowDirection=\u0022LeftToRight\u0022 NumberSubstitution.CultureSource=\u0022Text\u0022 NumberSubstitution.Substitution=\u0022AsCulture\u0022 FontFamily=\u0022Segoe UI\u0022 FontStyle=\u0022Normal\u0022 FontWeight=\u0022Normal\u0022 FontStretch=\u0022Normal\u0022 FontSize=\u002214\u0022 Foreground=\u0022#FF000000\u0022 Typography.StandardLigatures=\u0022True\u0022 Typography.ContextualLigatures=\u0022True\u0022 Typography.DiscretionaryLigatures=\u0022False\u0022 Typography.HistoricalLigatures=\u0022False\u0022 Typography.AnnotationAlternates=\u00220\u0022 Typography.ContextualAlternates=\u0022True\u0022 Typography.HistoricalForms=\u0022False\u0022 Typography.Kerning=\u0022True\u0022 Typography.CapitalSpacing=\u0022False\u0022 Typography.CaseSensitiveForms=\u0022False\u0022 Typography.StylisticSet1=\u0022False\u0022 Typography.StylisticSet2=\u0022False\u0022 Typography.StylisticSet3=\u0022False\u0022 Typography.StylisticSet4=\u0022False\u0022 Typography.StylisticSet5=\u0022False\u0022 Typography.StylisticSet6=\u0022False\u0022 Typography.StylisticSet7=\u0022False\u0022 Typography.StylisticSet8=\u0022False\u0022 Typography.StylisticSet9=\u0022False\u0022 Typography.StylisticSet10=\u0022False\u0022 Typography.StylisticSet11=\u0022False\u0022 Typography.StylisticSet12=\u0022False\u0022 Typography.StylisticSet13=\u0022False\u0022 Typography.StylisticSet14=\u0022False\u0022 Typography.StylisticSet15=\u0022False\u0022 Typography.StylisticSet16=\u0022False\u0022 Typography.StylisticSet17=\u0022False\u0022 Typography.StylisticSet18=\u0022False\u0022 Typography.StylisticSet19=\u0022False\u0022 Typography.StylisticSet20=\u0022False\u0022 Typography.Fraction=\u0022Normal\u0022 Typography.SlashedZero=\u0022False\u0022 Typography.MathematicalGreek=\u0022False\u0022 Typography.EastAsianExpertForms=\u0022False\u0022 Typography.Variants=\u0022Normal\u0022 Typography.Capitals=\u0022Normal\u0022 Typography.NumeralStyle=\u0022Normal\u0022 Typography.NumeralAlignment=\u0022Normal\u0022 Typography.EastAsianWidths=\u0022Normal\u0022 Typography.EastAsianLanguage=\u0022Normal\u0022 Typography.StandardSwashes=\u00220\u0022 Typography.ContextualSwashes=\u00220\u0022 Typography.StylisticAlternates=\u00220\u0022\u003E\u003CParagraph\u003E\u003CRun\u003E\u003C/Run\u003E\u003C/Paragraph\u003E\u003C/Section\u003E";

    private Note _note;
    private bool _open = false;
    private bool _pinned = false;
    private bool _show = true;
    private string _content = "";
    private string _theme = "";
    private DateTime _createdAt;

    public int Id { get { return _note.Id; } }

    public DateTime CreatedAt {
      get { return _createdAt; }
      set { _createdAt = value; OnPropertyChanged(); }
    }

    /// <summary>
    /// Whether this note should be shown in the note list.
    /// User can specify a search filter in which case a note
    /// may be filtered out from the note list.
    /// </summary>
    public bool Show {
      get { return _show; }
      set { _show = value; OnPropertyChanged(); }
    }

    public string Content {
      get { return _content; }
      set { _content = value; OnPropertyChanged(); }
    }

    public string Theme {
      get { return _theme; }
      set { _theme = value; OnPropertyChanged(); }
    }

    public bool Open {
      get { return _open; }
      set { _open = value; OnPropertyChanged(); }
    }

    public bool Pinned {
      get { return _pinned; }
      set { _pinned = value; OnPropertyChanged(); }
    }

    public bool IsEmpty {
      get {
        return string.IsNullOrWhiteSpace(Content) || Content == EMPTY_NOTE;
      }
    }

    public NoteViewModel(Note note) {
      _note = note;
      Content = note.Content;
      Theme = note.Theme;
      CreatedAt = note.CreatedAt;
      Pinned = note.Pinned;
    }

    public Note ToNote() {
      return new Note {
        Id = Id,
        Content = Content,
        Theme = Theme,
        CreatedAt = CreatedAt,
        Pinned = Pinned,
      };
    }
  }

  public class NotesViewModel : ObservableCollection<NoteViewModel> {
    public const int InvalidIndex = -1;


    private int GetIndex(int id) {
      for (var i = 0; i < Count; ++i) {
        if (this[i].Id == id) return i;
      }
      return InvalidIndex;
    }

    public NoteViewModel? GetById(int id) {
      var index = GetIndex(id);
      return index == InvalidIndex ? null : this[index];
    }

    public void RemoveById(int id) {
      var index = GetIndex(id);
      if (index == InvalidIndex) return;
      RemoveAt(index);
    }
  }

  public class SettingsViewModel : ViewModelBase {
    private Settings _settings;

    public SettingsViewModel(Settings settings) {
      _settings = settings;
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

  public class ViewModel : ViewModelBase {
    public RelayCommand CreateNoteCommand { get; private set; }
    public RelayCommand DeleteNoteCommand { get; private set; }
    public RelayCommand OpenNoteCommand { get; private set; }
    public RelayCommand CloseNoteCommand { get; private set; }
    public RelayCommand TogglePinnedCommand { get; private set; }

    public NotesViewModel Notes { get; }
    public SettingsViewModel Settings { get; }

    private Model model;

    public ViewModel(Model model) {
      Notes = new NotesViewModel();
      Settings = new SettingsViewModel(model.Settings);

      CreateNoteCommand = new RelayCommand(CreateNote);
      DeleteNoteCommand = new RelayCommand(DeleteNote);
      OpenNoteCommand = new RelayCommand(OpenNote);
      CloseNoteCommand = new RelayCommand(CloseNote);
      TogglePinnedCommand = new RelayCommand(TogglePinned);

      this.model = model;

      foreach (var note in model.Notes) {
        Notes.Add(new NoteViewModel(note));
      }
    }

    private void CreateNote(object parameter) {
      var note = model.CreateNote();
      Notes.Add(
        new NoteViewModel(note) {
          Open = true,
          Pinned = Settings.PinNewNote,
        }
      );
    }

    private void DeleteNote(object parameter) {
      var id = (int)parameter;
      Notes.RemoveById(id);
    }

    private void CloseNote(object parameter) {
      var id = (int)parameter;
      var note = Notes.GetById(id);
      if (note == null) return;

      if (note.IsEmpty) {
        Notes.RemoveById(id);
        return;
      }

      note.Open = false;

      // @NOTE: Force CollectionChanged event.
      var idx = Notes.IndexOf(note);
      Notes[idx] = note;
    }

    private void OpenNote(object parameter) {
      var id = (int)parameter;
      var note = Notes.GetById(id);
      if (note == null) return;

      note.Open = true;

      // @NOTE: Force CollectionChanged event.
      var idx = Notes.IndexOf(note);
      Notes[idx] = note;
    }

    private void TogglePinned(object parameter) {
      var id = (int)parameter;
      var note = Notes.GetById(id);
      if (note == null) return;

      note.Pinned = !note.Pinned;
    }

    // @TODO: Better way to move changes between view-model and the model.
    public void Flush() {
      model.Notes.Clear();
      foreach (var note in Notes) {
        model.Notes.Add(note.ToNote());
      }
    }
  }

}
