using System;
using System.Windows;
using System.Windows.Input;
using Sticky.DataAccess;

namespace Sticky.ViewModels {

  public class MainWindowViewModel : ViewModelBase {
    public ICommand CloseCommand { get; }
    public ICommand NewNoteCommand { get; }
    public ICommand SearchInNotesCommand { get; }
    public ICommand ActivatePreviousWindowCommand { get; }
    public ICommand ActivateNextWindowCommand { get; }

    public event Action CloseRequested;
    public event Action SearchInNotesRequested;
    public event Action<Window> ActivatePreviousWindowRequested;
    public event Action<Window> ActivateNextWindowRequested;

    private Database _db;

    public MainWindowViewModel(Database db) {
      this._db = db;

      CloseCommand = new RelayCommand(() => CloseRequested?.Invoke());
      NewNoteCommand = new RelayCommand(() => _db.AddNote(NoteFactory.CreateNote(_db.GetSettings()))); // @TODO: Apply settings...

      SearchInNotesCommand = new RelayCommand(() => SearchInNotesRequested?.Invoke());
      ActivatePreviousWindowCommand = new RelayCommand((param) => ActivatePreviousWindowRequested?.Invoke((Window)param));
      ActivateNextWindowCommand = new RelayCommand((param) => ActivateNextWindowRequested?.Invoke((Window)param));
    }
  }

}
