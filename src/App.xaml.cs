using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace Sticky {

  public class State {
    public List<Note> Notes { get; set; } = new List<Note>();

    public State() {
      var notes = Notes;
      notes.Add(new Note(){
        Id = 1,
        CreatedAt = new DateTime(2022, 6, 8, 14, 30, 0),
        Content = "<FlowDocument xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" PagePadding=\"5,0,5,0\" AllowDrop=\"True\"><Paragraph><Bold>Note 1</Bold></Paragraph><Paragraph>This is note 1.</Paragraph></FlowDocument>"
      });
      notes.Add(new Note(){
        Id = 2,
        CreatedAt = new DateTime(2022, 6, 7, 15, 30, 0),
        Content = "<FlowDocument xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" PagePadding=\"5,0,5,0\" AllowDrop=\"True\"><Paragraph><Bold>Note 2</Bold></Paragraph><Paragraph>This is note 2.</Paragraph></FlowDocument>"
      });
    }
  }

  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application {
    private Mutex? singleInstanceMutex;

    public State state = new State();

    public App() {
      Services = CreateServices();
      InitializeComponent();
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
