using System;
using System.Windows;
using System.Windows.Input;
using ModernWpf;
using Sticky.DataAccess;
using Sticky.ViewModels;

namespace Sticky {

  public partial class App : Application {
    private Database _db;
    private Settings _settings;
    public ThemeService Themes = new ThemeService();

    public App(DataAccess.Database db) {
      this._db = db;
      this._settings = _db.GetSettings();
      SetBaseTheme(_settings.BaseTheme);

      AppContext.SetSwitch("Switch.System.Windows.Controls.Text.UseAdornerForTextboxSelectionRendering", false);

      _db.NoteAdded += (sender, e) => {
        var note = e.AddedNote;
        if (note.IsOpen) OpenNoteWindow(new NoteWindowViewModel(note, _db));
      };

      _db.NoteDeleted += (sender, e) => CloseNoteWindow(e.DeletedNote.Id);

      _db.NoteModified += (sender, e) => {
        var note = e.ModifiedNote;

         // @TODO
        if (note.IsOpen) {
          var window = FindNoteWindow(note.Id);
          if (window != null) window.Activate();
          else OpenNoteWindow(new NoteWindowViewModel(note, _db));
        } else {
          CloseNoteWindow(note.Id);
        }
      };

      _db.SettingsModified += (sender, e) => {
        _settings = e.NewSettings;
        SetBaseTheme(_settings.BaseTheme);
      };
    }

    protected override void OnStartup(StartupEventArgs e) {
      base.OnStartup(e);

      var mainWindow = new MainWindow(_db);
      mainWindow.Show();

#if !DEBUG
      AppDomain.CurrentDomain.UnhandledException += (sender, e) => {
        MessageBox.Show("An unexpected error has occurred. Sticky Notes is going to terminate.", "Sticky Notes", MessageBoxButton.OK, MessageBoxImage.Error);
        Environment.Exit(0);
      };
#endif

      ThemeManager.Current.ApplicationTheme = _settings.BaseTheme.ToApplicationTheme();
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

    private void OpenNoteListExecuted(object sender, ExecutedRoutedEventArgs e) {
      var mainWindow = MainWindow;
      if (mainWindow != null) {
        mainWindow.Activate();
      } else {
        MainWindow = new MainWindow(_db);
        MainWindow.Show();
      }
    }

    private void SetBaseTheme(BaseTheme theme) {
      if (MainWindow == null) return;

      MainWindow.ClearValue(ThemeManager.RequestedThemeProperty);
      var applicationTheme = theme.ToApplicationTheme();

      if (applicationTheme != ThemeManager.Current.ActualApplicationTheme) {
        ThemeManager.Current.ApplicationTheme = applicationTheme;
      }
    }

    #if false
    async private void DeleteNoteExecuted(object sender, ExecutedRoutedEventArgs e) {
      var confirmDelete = async () => {
        if (!ViewModel.Settings.ConfirmBeforeDelete) return true;

        var dialog = new ConfirmDeleteDialog();
        var result = await dialog.ShowAsync();

        var doDelete = (result == ContentDialogResult.Primary);
        return doDelete;
      };

      if (await confirmDelete()) {
        ViewModel.DeleteNoteCommand.Execute(e.Parameter);
      }
    }
    #endif
  }
}
