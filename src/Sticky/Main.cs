using System.Threading;
using System.Windows;

namespace Sticky {

  public class EntryPoint {

    // https://stackoverflow.com/questions/14506406/wpf-single-instance-best-practices
    private static void ToForeground(Window window) {
      if (window.WindowState == WindowState.Minimized || window.Visibility == Visibility.Hidden) {
        window.Show();
        window.WindowState = WindowState.Normal;
      }

      window.Activate();
      window.Topmost = true;
      window.Topmost = false;
      window.Focus();
    }

    [System.STAThreadAttribute()]
    public static void Main() {
      var eventHandle = new EventWaitHandle(false, EventResetMode.AutoReset, "8a50bf4b-577c-4ce4-aff6-80706452be5a");

      var created = false;
      var mutex = new Mutex(true, "cbe6e195-20b6-4950-97fa-6da85c3715f8", out created);
      if (!created) {
        // @NOTE: Other instance is already running. Set
        // event to bring it to the foreground.
        eventHandle.Set();
        return;
      }

      // @TODO: Exit the thread in a clean way.
      var thread = new Thread(() =>
        {
          while (eventHandle.WaitOne()) {
            App.Current.Dispatcher.BeginInvoke(() => ToForeground(App.Current.MainWindow));
          }
        }
      );
      thread.IsBackground = true;
      thread.Start();

      var app = new App();
      app.InitializeComponent();
      app.Run();
    }

  }

}
