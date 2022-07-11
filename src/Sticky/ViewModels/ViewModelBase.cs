using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sticky.ViewModels {

  /// <summary>
  /// Base class for view models providing property changed notifications.
  /// </summary>
  public class ViewModelBase : INotifyPropertyChanged {
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? name = null) {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null) {
      if (object.Equals(storage, value)) return false;

      storage = value;
      OnPropertyChanged(propertyName);

      return true;
    }
  }

}
