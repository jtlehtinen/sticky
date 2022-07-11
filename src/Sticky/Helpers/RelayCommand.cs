using System;
using System.Windows.Input;

namespace Sticky {

  // https://docs.microsoft.com/en-us/windows/communitytoolkit/mvvm/relaycommand
  public class RelayCommand : ICommand {
    private readonly Action<object> _execute;
    private readonly Predicate<object> _canExecute;

    public RelayCommand(Action execute) {
      _execute = x => execute.Invoke();
      _canExecute = x => true;
    }

    public RelayCommand(Action<object> execute) {
      _execute = execute;
      _canExecute = x => true;
    }

    public RelayCommand(Action<object> execute, Predicate<object> canExecute) {
      _execute = execute;
      _canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public void Execute(object parameter) => _execute(parameter);
    public bool CanExecute(object parameter) => _canExecute(parameter);
  }

}
