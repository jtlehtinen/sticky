using Microsoft.UI.Xaml.Controls;

using Sticky.ViewModels;

namespace Sticky.Views;

public sealed partial class BlankPage : Page {
  public BlankViewModel ViewModel {
    get;
  }

  public BlankPage() {
    ViewModel = App.GetService<BlankViewModel>();
    InitializeComponent();
  }
}
