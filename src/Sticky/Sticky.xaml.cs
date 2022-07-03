using System;
using System.Windows;
using Sticky.DataAccess;
using Sticky.Helpers;
using Sticky.ViewModels;

namespace Sticky {

  public partial class App : Application {
    private Database _db;
    public Themes Themes = new Themes();

    public App(Database db) {
      this._db = db;

      _db.NoteAdded += (sender, e) => {
        var note = e.AddedNote;
        if (note.IsOpen) {
          var vm = new NoteWindowViewModel(note, _db);
          vm.OpenNoteListRequested += () => ActivateMainWindow();
          WindowHelper.OpenNoteWindow(vm, MainWindow);
        }
      };

      _db.NoteDeleted += (sender, e) => WindowHelper.CloseNoteWindow(Windows, e.DeletedNote.Id);

      _db.NoteModified += (sender, e) => {
        var note = e.ModifiedNote;

         // @TODO
        if (note.IsOpen) {
          var window = WindowHelper.FindNoteWindow(Windows, note.Id);
          if (window != null) window.Activate();
          else {
            var vm = new NoteWindowViewModel(note, _db);
            vm.OpenNoteListRequested += () => ActivateMainWindow();
            WindowHelper.OpenNoteWindow(vm, MainWindow);
          }
        } else {
          WindowHelper.CloseNoteWindow(Windows, note.Id);
        }
      };

      _db.SettingsModified += (sender, e) => Themes.SetBaseTheme(e.NewSettings.BaseTheme);
    }

    public new static App Current => (App)Application.Current;

    public new MainWindow? MainWindow {
      get { return base.MainWindow as MainWindow; }
      set { base.MainWindow = value; }
    }

    public void ActivateMainWindow() {
      if (MainWindow == null) {
        MainWindow = new MainWindow(_db);
      }

      MainWindow.Show();
      MainWindow.Activate();
    }

    protected override void OnStartup(StartupEventArgs e) {
      AppContext.SetSwitch("Switch.System.Windows.Controls.Text.UseAdornerForTextboxSelectionRendering", false);

      base.OnStartup(e);

      var mainWindow = new MainWindow(_db);
      mainWindow.Show();

      Themes.SetBaseTheme(_db.GetSettings().BaseTheme);
    }
  }

}
