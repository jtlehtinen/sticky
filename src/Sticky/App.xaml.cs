using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using ModernWpf;

namespace Sticky {

  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application {
    private Mutex? singleInstanceMutex;
    private List<Window> noteWindows = new();

    public App() {
      // @NOTE: TextBox SelectionTextBrush doesn't work without this.
      // @TODO: RichTextBox SelectionTextBrush doesn't work event with this.
      // https://github.com/microsoft/dotnet/blob/master/Documentation/compatibility/wpf-SelectionTextBrush-property-for-non-adorner-selection.md
      // https://github.com/Microsoft/dotnet/blob/master/Documentation/compatibility/wpf-TextBox-PasswordBox-text-selection-does-not-follow-system-colors.md
      AppContext.SetSwitch("Switch.System.Windows.Controls.Text.UseAdornerForTextboxSelectionRendering", false);

      Services = CreateServices();
      InitializeComponent();
    }

    public IServiceProvider Services { get; private set; }

    public new static App Current => (App)Application.Current;

    public new MainWindow MainWindow {
      get { return (MainWindow)base.MainWindow; }
      set { base.MainWindow = value; }
    }

    private IServiceProvider CreateServices() {
      var services = new ServiceCollection();
      services.AddService(new ThemeService());
      services.AddService(new NoteService());
      return services;
    }

    private void Main(object sender, StartupEventArgs args) {
      var createdNewMutex = false;
      singleInstanceMutex = new Mutex(true, "cbe6e195-20b6-4950-97fa-6da85c3715f8", out createdNewMutex);
      if (!createdNewMutex) {
        Shutdown();
        return;
      }

      Exit += (sender, e) => Services.GetService<NoteService>()?.Commit();

      Commands.Register(typeof(Window), Commands.ToggleAppTheme, ToggleAppThemeExecuted);
      Commands.Register(typeof(Window), Commands.NewNote, NewNoteExecuted);

      var window = new MainWindow();
      window.Show();

      var noteService = Services.GetService<NoteService>();
      noteService?.GetNotes().ForEach(note => OpenNote(note, MainWindow));
    }

    private void OpenNote(Note? note, Window? positionNextTo = null) {
      var window = note != null ? new NoteWindow(note) : new NoteWindow();

      if (positionNextTo != null) {
        window.Left = positionNextTo.Left + positionNextTo.Width + 12;
        window.Top = positionNextTo.Top;
      }

      noteWindows.Add(window);
      window.Show();
    }

    private void NewNoteExecuted(object sender, ExecutedRoutedEventArgs e) {
      System.Console.WriteLine("NewNoteCommandExecuted()");
      OpenNote(null, e.Parameter as Window);
		}

    private void ToggleAppThemeExecuted(object sender, ExecutedRoutedEventArgs e) {
      System.Console.WriteLine("ToggleAppThemeCommandExecuted()");

      var window = MainWindow;
      window.ClearValue(ThemeManager.RequestedThemeProperty);

      var isDark = ThemeManager.Current.ActualApplicationTheme == ApplicationTheme.Dark;
      var newTheme = isDark ? ApplicationTheme.Light : ApplicationTheme.Dark;
      ThemeManager.Current.ApplicationTheme = newTheme;
    }

    private void ToggleWindowThemeCommandExecuted(object sender, ExecutedRoutedEventArgs e) {
      #if false
      var newTheme = ThemeManager.GetActualTheme(this) == ElementTheme.Light ? ElementTheme.Dark : ElementTheme.Light;
      ThemeManager.SetRequestedTheme(this, newTheme);
      #endif
    }
  }
}
