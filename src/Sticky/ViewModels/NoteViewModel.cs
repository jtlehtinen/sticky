using System;
using System.Windows.Input;
using Sticky.DataAccess;

namespace Sticky.ViewModels {

  public class NoteViewModel : ViewModelBase {
    public ICommand OpenCommand { get; }
    public ICommand CloseCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand OpenContextMenuCommand { get; }

    public event Action OpenContextMenuRequested;

    private Note _note;
    private Database _db;

    public NoteViewModel(Note note, Database db) {
      this._note = note;
      this._db = db;

      CloseCommand = new RelayCommand(() => {
        // @NOTE: When an empty note is closed it is removed.
        if (IsEmpty) {
          _db.DeleteNote(_note);
        } else {
          IsOpen = false;
        }
      });

      OpenCommand = new RelayCommand(() => IsOpen = true);
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
      OpenContextMenuCommand = new RelayCommand(() => OpenContextMenuRequested?.Invoke());

      PropertyChanged += (sender, e) => _db.UpdateNote(_note);
    }

    public int Id { get { return _note.Id; } }

    public DateTime CreatedAt {
      get { return _note.CreatedAt; }
    }

    public DateTime UpdatedAt {
      get { return _note.UpdatedAt; }
    }

    public bool Show { get; } = true;

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

    public bool IsEmpty {
      get { return _note.IsEmpty(); }
    }
  }

}
