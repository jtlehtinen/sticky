using System;
using System.Threading;
using Sticky.DataAccess;

namespace Sticky {

  public class EntryPoint {

    private static void SetExceptionHandler() {
#if !DEBUG
      AppDomain.CurrentDomain.UnhandledException += (sender, e) => {
        MessageBox.Show("An unexpected error has occurred. Sticky Notes is going to terminate.", "Sticky Notes", MessageBoxButton.OK, MessageBoxImage.Error);
        Environment.Exit(0);
      };
#endif
    }

    [System.STAThreadAttribute()]
    public static void Main() {
      var eventHandle = new EventWaitHandle(false, EventResetMode.AutoReset, "8a50bf4b-577c-4ce4-aff6-80706452be5a");

      var mutex = new Mutex(true, "cbe6e195-20b6-4950-97fa-6da85c3715f8", out bool created);
      if (!created) {
        // @NOTE: Activate already running instance.
        eventHandle.Set();
        Environment.Exit(0);
      }

      var thread = new Thread(() => {
          while (eventHandle.WaitOne()) {
            App.Current.Dispatcher.BeginInvoke(() => App.Current.ActivateMainWindow());
          }
        }
      );
      thread.IsBackground = true;
      thread.Start();

      SetExceptionHandler();

      var db = new Database();
      var app = new App(db);

      app.InitializeComponent();
      app.Run();
    }

  }

}
