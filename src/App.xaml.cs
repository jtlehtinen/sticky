using System.Windows;

namespace Sticky {
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application {
    private void Main(object sender, StartupEventArgs args) {
      //var commandLineArguments = args.Args;

      var window = new MainWindow();
      window.Show();
    }

    private void ExceptionCatchAll(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs args) {
      //args.Handled = true;
    }
  }
}
