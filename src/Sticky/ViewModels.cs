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
    private Note _note;
    private bool _open = false;
    private bool _pinned = false;
    private string _content = "";
    private string _theme = "";
    private DateTime _createdAt;

    public int Id { get { return _note.Id; } }

    public DateTime CreatedAt {
      get { return _createdAt; }
      set { _createdAt = value; OnPropertyChanged(); }
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

    public NoteViewModel(Note note) {
      _note = note;
      Content = note.Content;
      Theme = note.Theme;
      CreatedAt = note.CreatedAt;
    }

    public Note ToNote() {
      return new Note {
        Id = Id,
        Content = Content,
        Theme = Theme,
        CreatedAt = CreatedAt,
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

  public class ViewModel : ViewModelBase {
    public RelayCommand CreateNoteCommand { get; private set; }
    public RelayCommand DeleteNoteCommand { get; private set; }
    public RelayCommand OpenNoteCommand { get; private set; }
    public RelayCommand CloseNoteCommand { get; private set; }

    public NotesViewModel Notes { get; } = new NotesViewModel();

    private Model model;

    public ViewModel(Model model) {
      CreateNoteCommand = new RelayCommand(CreateNote);
      DeleteNoteCommand = new RelayCommand(DeleteNote);
      OpenNoteCommand = new RelayCommand(OpenNote);
      CloseNoteCommand = new RelayCommand(CloseNote);

      this.model = model;

      foreach (var note in model.Notes) {
        Notes.Add(new NoteViewModel(note));
      }
    }

    private void CreateNote(object parameter) {
      var note = model.CreateNote();
      Notes.Add(new NoteViewModel(note) { Open = true });
    }

    private void DeleteNote(object parameter) {
      var id = (int)parameter;
      Notes.RemoveById(id);
    }

    private void CloseNote(object parameter) {
      var id = (int)parameter;
      var note = Notes.GetById(id);
      if (note == null) return;

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

    // @TODO: Better way to move changes between view-model and the model.
    public void Flush() {
      model.Notes.Clear();
      foreach (var note in Notes) {
        model.Notes.Add(note.ToNote());
      }
    }
  }

}
