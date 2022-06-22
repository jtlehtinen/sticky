using System;
using System.Collections.Specialized;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using ModernWpf;

namespace Sticky {

  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application {
    public Model Model;
    public ViewModel ViewModel;
    public ThemeService Themes = new ThemeService();

    public App() {
      Model = new Model();
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

      Exit += (sender, e) => Model.Save();

      Commands.Register(typeof(Window), Commands.ChangeAppTheme, ChangeAppThemeExecuted);
      Commands.Register(typeof(Window), Commands.DeleteNote, DeleteNoteExecuted);
      Commands.Register(typeof(Window), Commands.NewNote, NewNoteExecuted);
      Commands.Register(typeof(Window), Commands.CloseNote, CloseNoteExecuted);
      Commands.Register(typeof(Window), Commands.OpenNote, OpenNoteExecuted);
      Commands.Register(typeof(Window), Commands.OpenNoteList, OpenNoteListExecuted);
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
        if (window is NoteWindow noteWindow && noteWindow.Note == note) {
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

    private void ChangeAppThemeExecuted(object sender, ExecutedRoutedEventArgs e) {
      // @TODO: Save the setting.
      if (MainWindow == null) return;

      MainWindow.ClearValue(ThemeManager.RequestedThemeProperty);

      var ToEnum = (string theme) => {
        if (theme == "Dark") return ApplicationTheme.Dark;
        if (theme == "Light") return ApplicationTheme.Light;
        if (theme == "System") return ApplicationTheme.Light; // @TODO:
        throw new ArgumentException("Unknown theme: " + theme);
      };

      var newTheme = ToEnum((string)e.Parameter);
      if (newTheme != ThemeManager.Current.ActualApplicationTheme) {
        ThemeManager.Current.ApplicationTheme = newTheme;
      }
    }

    private void CloseNoteExecuted(object sender, ExecutedRoutedEventArgs e) => ViewModel.CloseNoteCommand.Execute(e.Parameter);
    private void OpenNoteExecuted(object sender, ExecutedRoutedEventArgs e) => ViewModel.OpenNoteCommand.Execute(e.Parameter);

    private void NewNoteExecuted(object sender, ExecutedRoutedEventArgs e) => ViewModel.CreateNoteCommand.Execute(null);
    private void DeleteNoteExecuted(object sender, ExecutedRoutedEventArgs e) => ViewModel.DeleteNoteCommand.Execute(e.Parameter);
  }
}
