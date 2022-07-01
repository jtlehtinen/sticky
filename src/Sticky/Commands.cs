using System;
using System.Windows.Input;

namespace Sticky {

  // https://docs.microsoft.com/en-us/windows/communitytoolkit/mvvm/relaycommand
  public class RelayCommand : ICommand {
    private readonly Action<object> _execute;
    private readonly Predicate<object> _canExecute;

    public RelayCommand(Action execute) {
      _canExecute = x => true;
      _execute = x => execute.Invoke();
    }

    public RelayCommand(Action<object> execute) : this(execute, x => true) { }

    public RelayCommand(Action<object> execute, Predicate<object> canExecute) {
      _canExecute = canExecute;
      _execute = execute;
    }

    public event EventHandler CanExecuteChanged {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public void Execute(object parameter) => _execute(parameter);
    public bool CanExecute(object parameter) => _canExecute(parameter);
  }

  public static class Commands {
    public static RoutedCommand OpenNoteList = new RoutedCommand("OpenNoteList", typeof(Commands));

    public static RoutedCommand About { get; } = new RoutedCommand("About", typeof(Commands));
    public static RoutedCommand NewNote { get; } = new RoutedCommand("NewNote", typeof(Commands));
    public static RoutedCommand DeleteNote { get; } = new RoutedCommand("DeleteNote", typeof(Commands));
    public static RoutedCommand CloseNote { get; } = new RoutedCommand("CloseNote", typeof(Commands));
    public static RoutedCommand OpenNote { get; } = new RoutedCommand("OpenNote", typeof(Commands));
    public static RoutedCommand ChangeAppTheme { get; } = new RoutedCommand("ChangeAppTheme", typeof(Commands));

    public static RoutedCommand ChangeNoteTheme { get; } = new RoutedCommand("ChangeNoteTheme", typeof(Commands));
    public static RoutedCommand ShowMenu { get; } = new RoutedCommand("ShowMenu", typeof(Commands));

    public static RoutedCommand OpenContextMenu { get; } = new RoutedCommand("OpenContextMenu", typeof(Commands));

    public static void Register(Type type, RoutedCommand command, ExecutedRoutedEventHandler executed) {
      CommandManager.RegisterClassCommandBinding(type, new CommandBinding(command, executed));
    }
  }

}
