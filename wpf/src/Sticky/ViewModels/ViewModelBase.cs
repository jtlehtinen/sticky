using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sticky.ViewModels {

  /// <summary>
  /// Base class for view models providing property changed notifications.
  /// </summary>
  public class ViewModelBase : INotifyPropertyChanged {
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null) {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
  }

}
