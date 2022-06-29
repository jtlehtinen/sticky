using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ModernWpf;
using ModernWpf.Controls;

namespace Sticky {

  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application {
    public Model Model;
    public ViewModel ViewModel;
    public ThemeService Themes = new ThemeService();

    public App() {
      Model = Import.FromJson("sticky.json") ?? new Model();
      ViewModel = new ViewModel(Model);

      ViewModel.PropertyChanged += (sender, e) => {
        System.Console.WriteLine("ViewModel.PropertyChanged: " + e.PropertyName);
      };

      ViewModel.Notes.CollectionChanged += (sender, e) => {
        System.Console.WriteLine("ViewModel.Notes.CollectionChanged: " + e.Action);

        switch (e.Action) {
          case NotifyCollectionChangedAction.Add: OnNoteCreated(e.NewItems[0] as NoteViewModel); break;
          case NotifyCollectionChangedAction.Remove: OnNoteDeleted(e.OldItems[0] as NoteViewModel); break;
          case NotifyCollectionChangedAction.Replace: OnNoteReplaced(e.NewItems[0] as NoteViewModel); break;
        }
      };

      // @NOTE: TextBox SelectionTextBrush doesn't work without this.
      // @TODO: RichTextBox SelectionTextBrush doesn't work event with this.
      // https://github.com/microsoft/dotnet/blob/master/Documentation/compatibility/wpf-SelectionTextBrush-property-for-non-adorner-selection.md
      // https://github.com/Microsoft/dotnet/blob/master/Documentation/compatibility/wpf-TextBox-PasswordBox-text-selection-does-not-follow-system-colors.md
      AppContext.SetSwitch("Switch.System.Windows.Controls.Text.UseAdornerForTextboxSelectionRendering", false);

      Exit += (sender, e) => {
        ViewModel.Flush();
        Export.ToJson("sticky.json", Model);
      };

      Commands.Register(typeof(Window), Commands.ChangeAppTheme, ChangeAppThemeExecuted);
      Commands.Register(typeof(Window), Commands.DeleteNote, DeleteNoteExecuted);
      Commands.Register(typeof(Window), Commands.NewNote, NewNoteExecuted);
      Commands.Register(typeof(Window), Commands.CloseNote, CloseNoteExecuted);
      Commands.Register(typeof(Window), Commands.OpenNote, OpenNoteExecuted);
      Commands.Register(typeof(Window), Commands.OpenNoteList, OpenNoteListExecuted);
      Commands.Register(typeof(Window), Commands.TogglePinned, TogglePinnedExecuted);
    }

    protected override void OnStartup(StartupEventArgs e) {
      base.OnStartup(e);

#if !DEBUG
      AppDomain.CurrentDomain.UnhandledException += (sender, e) => {
        MessageBox.Show("An unexpected error has occurred. Sticky Notes is going to terminate.", "Sticky Notes", MessageBoxButton.OK, MessageBoxImage.Error);
        Environment.Exit(0);
      };
#endif

      ThemeManager.Current.ApplicationTheme = ToApplicationTheme(ViewModel.Settings.BaseTheme);
    }

    public new static App Current => (App)Application.Current;

    public new MainWindow? MainWindow {
      get { return base.MainWindow as MainWindow; }
      set { base.MainWindow = value; }
    }

    private void OnNoteCreated(NoteViewModel note) {
      if (note == null) return;

      if (note.Open) OpenNoteWindow(note);
    }

    private void OnNoteDeleted(NoteViewModel note) {
      if (note == null) return;

      CloseNoteWindow(note);
    }

    private void OnNoteReplaced(NoteViewModel note) {
      if (note == null) return;

      if (note.Open) {

        var window = FindNoteWindow(note);
        if (window != null) window.Activate();
        else OpenNoteWindow(note);

      } else {
        CloseNoteWindow(note);
      }
    }

    private void OpenNoteWindow(NoteViewModel note) {
      var window = new NoteWindow(note);

      var mainWindow = MainWindow;
      if (mainWindow != null) {
        window.Left = MainWindow.Left + MainWindow.Width + 12;
        window.Top = MainWindow.Top;
      }

      window.Show();
    }

    private Window? FindNoteWindow(NoteViewModel note) {
      foreach (var window in Windows) {
        if (window is NoteWindow noteWindow && noteWindow.DataContext == note) {
          return noteWindow;
        }
      }
      return null;
    }

    private void CloseNoteWindow(NoteViewModel note) {
      var window = FindNoteWindow(note);
      if (window != null) window.Close();
    }

    private void OpenNoteListExecuted(object sender, ExecutedRoutedEventArgs e) {
      var mainWindow = MainWindow;
      if (mainWindow != null) {
        mainWindow.Activate();
      } else {
        MainWindow = new MainWindow();
        MainWindow.Show();
      }
    }

    private ApplicationTheme? ToApplicationTheme(BaseTheme theme) {
      if (theme == BaseTheme.Dark) return ApplicationTheme.Dark;
      if (theme == BaseTheme.Light) return ApplicationTheme.Light;
      if (theme == BaseTheme.System) return null;
      throw new ArgumentException("theme");
    }

    private void ChangeAppThemeExecuted(object sender, ExecutedRoutedEventArgs e) {
      // @TODO: Save the setting.
      if (MainWindow == null) return;

      MainWindow.ClearValue(ThemeManager.RequestedThemeProperty);
      // @TODO: Figure out system theme...
      ViewModel.Settings.BaseTheme = (BaseTheme)e.Parameter;

      var newTheme = ToApplicationTheme(ViewModel.Settings.BaseTheme);
      if (newTheme != ThemeManager.Current.ActualApplicationTheme) {
        ThemeManager.Current.ApplicationTheme = newTheme;
      }
    }

    private void CloseNoteExecuted(object sender, ExecutedRoutedEventArgs e) {
      ViewModel.CloseNoteCommand.Execute(e.Parameter);
    }

    private void OpenNoteExecuted(object sender, ExecutedRoutedEventArgs e) {
      ViewModel.OpenNoteCommand.Execute(e.Parameter);
    }

    private void NewNoteExecuted(object sender, ExecutedRoutedEventArgs e) {
      ViewModel.CreateNoteCommand.Execute(null);
    }

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

    private void TogglePinnedExecuted(object sender, ExecutedRoutedEventArgs e) {
      ViewModel.TogglePinnedCommand.Execute(e.Parameter);
    }
  }
}
