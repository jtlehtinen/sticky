using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Sticky.DataAccess;

namespace Sticky.ViewModels {

  public class MainPageViewModel : ViewModelBase {
    public ICommand CloseCommand { get; }
    public ICommand NewNoteCommand { get; }
    public ICommand OpenSettingsPageCommand { get; }

    public ObservableCollection<NoteViewModel> AllNotes { get; private set; }

    private Database _db;

    public MainPageViewModel(Database db) {
      this._db = db;

      db.NoteAdded += OnNoteAddedToDatabase;
      db.NoteDeleted += OnNoteRemovedFromDatabase;
      db.NoteModified += OnNoteModifiedInDatabase;

      CloseCommand = new RelayCommand(param => ((Window)param).Close());
      OpenSettingsPageCommand = new RelayCommand((param) => ((MainWindow)param).Navigate(PageType.Settings));
      NewNoteCommand = new RelayCommand(() => _db.AddNote(new Note())); // @TODO: Apply settings...

      LoadAllNotes();
    }

    private void LoadAllNotes() {
      var all = new List<NoteViewModel>();

      foreach (var note in _db.GetNotes()) {
        all.Add(new NoteViewModel(note, _db));
      }

      foreach (var note in all) {
        note.PropertyChanged += OnNoteViewModelPropertyChanged;
      }

      AllNotes = new ObservableCollection<NoteViewModel>(all);
      AllNotes.CollectionChanged += OnCollectionChanged;
    }

    private void OnNoteAddedToDatabase(object sender, NoteAddedEventArgs e) {
      var vm = new NoteViewModel(e.AddedNote, _db);
      vm.PropertyChanged += OnNoteViewModelPropertyChanged;
      AllNotes.Add(vm);
    }

    private void OnNoteRemovedFromDatabase(object sender, NoteDeletedEventArgs e) {
      AllNotes.RemoveAll(note => note.Id == e.DeletedNote.Id);
    }

    private void OnNoteModifiedInDatabase(object sender, NoteModifiedEventArgs e) {
      var note = e.ModifiedNote;
      for (var i = 0; i < AllNotes.Count; ++i) {
        if (AllNotes[i].Id == note.Id) {
          var vm = new NoteViewModel(note, _db);
          vm.PropertyChanged += OnNoteViewModelPropertyChanged;

          AllNotes[i] = vm;
          break;
        }
      }
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
      if (e.NewItems != null && e.NewItems.Count != 0) {
        foreach (NoteViewModel vm in e.NewItems)
          vm.PropertyChanged += OnNoteViewModelPropertyChanged;
      }

      if (e.OldItems != null && e.OldItems.Count != 0) {
        foreach (NoteViewModel vm in e.OldItems)
          vm.PropertyChanged -= OnNoteViewModelPropertyChanged;
      }
    }

    void OnNoteViewModelPropertyChanged(object sender, PropertyChangedEventArgs e) {

    }
  }

}
