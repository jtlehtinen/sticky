using System;
using System.Windows;
using System.Windows.Input;
using ModernWpf;
using Sticky.DataAccess;
using Sticky.ViewModels;

namespace Sticky {

  public partial class App : Application {
    private Database _db;
    public ThemeService Themes = new ThemeService();

    public App(Database db) {
      this._db = db;

      AppContext.SetSwitch("Switch.System.Windows.Controls.Text.UseAdornerForTextboxSelectionRendering", false);

      _db.NoteAdded += (sender, e) => {
        var note = e.AddedNote;
        if (note.IsOpen) {
          var vm = new NoteWindowViewModel(note, _db);
          vm.OpenNoteListRequested += () => OpenNoteList();
          OpenNoteWindow(vm);
        }
      };

      _db.NoteDeleted += (sender, e) => CloseNoteWindow(e.DeletedNote.Id);

      _db.NoteModified += (sender, e) => {
        var note = e.ModifiedNote;

         // @TODO
        if (note.IsOpen) {
          var window = FindNoteWindow(note.Id);
          if (window != null) window.Activate();
          else {
            var vm = new NoteWindowViewModel(note, _db);
            vm.OpenNoteListRequested += () => OpenNoteList();
            OpenNoteWindow(vm);
          }
        } else {
          CloseNoteWindow(note.Id);
        }
      };

      _db.SettingsModified += (sender, e) => SetBaseTheme(e.NewSettings.BaseTheme);
    }

    public void ActivateMainWindow() {
      OpenNoteList();
    }

    private void OpenNoteList() {
      if (MainWindow == null) {
        MainWindow = new MainWindow(_db);
      }
      MainWindow.Show();
      MainWindow.Activate();
    }

    protected override void OnStartup(StartupEventArgs e) {
      base.OnStartup(e);

      var mainWindow = new MainWindow(_db);
      mainWindow.Show();

      SetBaseTheme(_db.GetSettings().BaseTheme);
    }

    public new static App Current => (App)Application.Current;

    public new MainWindow? MainWindow {
      get { return base.MainWindow as MainWindow; }
      set { base.MainWindow = value; }
    }

    private void OpenNoteWindow(NoteWindowViewModel vm) {
      var window = new NoteWindow(vm);

      var mainWindow = MainWindow;
      if (mainWindow != null) {
        window.Left = MainWindow.Left + MainWindow.Width + 12;
        window.Top = MainWindow.Top;
      }

      window.Show();
    }

    private Window? FindNoteWindow(int noteId) {
      foreach (var window in Windows) {
        if (window is NoteWindow noteWindow && noteWindow.DataContext is NoteWindowViewModel noteWindowViewModel && noteWindowViewModel.Id == noteId) {
          return noteWindow;
        }
      }
      return null;
    }

    private void CloseNoteWindow(int noteId) {
      var window = FindNoteWindow(noteId);
      if (window != null) window.Close();
    }

    private void SetBaseTheme(BaseTheme theme) {
      if (MainWindow != null) {
        MainWindow.ClearValue(ThemeManager.RequestedThemeProperty);
      }

      var appTheme = theme.ToApplicationTheme();
      if (appTheme != ThemeManager.Current.ActualApplicationTheme) {
        ThemeManager.Current.ApplicationTheme = appTheme;
      }
    }
  }
}
