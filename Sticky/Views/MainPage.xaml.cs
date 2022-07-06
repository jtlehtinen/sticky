using Microsoft.UI.Xaml.Controls;

using Sticky.ViewModels;

namespace Sticky.Views;

public sealed partial class MainPage : Page {
  public MainViewModel ViewModel {
    get;
  }

  public MainPage() {
    ViewModel = App.GetService<MainViewModel>();
    InitializeComponent();
  }
}
