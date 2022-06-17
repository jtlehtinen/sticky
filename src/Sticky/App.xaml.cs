using System;
using System.Threading;
using System.Windows;

namespace Sticky {

  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application {
    private Mutex? singleInstanceMutex;

    public App() {
      // @NOTE: TextBox SelectionTextBrush doesn't work without this.
      // @TODO: RichTextBox SelectionTextBrush doesn't work at all.
      // https://github.com/microsoft/dotnet/blob/master/Documentation/compatibility/wpf-SelectionTextBrush-property-for-non-adorner-selection.md
      // https://github.com/Microsoft/dotnet/blob/master/Documentation/compatibility/wpf-TextBox-PasswordBox-text-selection-does-not-follow-system-colors.md
      AppContext.SetSwitch("Switch.System.Windows.Controls.Text.UseAdornerForTextboxSelectionRendering", false);

      Services = CreateServices();
      InitializeComponent();

      Exit += (sender, e) => Services.GetService<NoteService>()?.Commit();
    }

    public IServiceProvider Services { get; }

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

      var window = new MainWindow();
      window.Show();
    }
  }
}
