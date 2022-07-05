using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Sticky.ViewModels;

namespace Sticky {

  public partial class SettingsPage : UserControl {
    private DragBehavior _drag;

    public SettingsPage(SettingsPageViewModel vm) {
      DataContext = vm;
      InitializeComponent();

      _drag = new DragBehavior(this);
    }

    private void OnThemeChanged(object sender, SelectionChangedEventArgs e) {
      if (e.AddedItems.Count == 0 || e.AddedItems[0] == null) return;

      var element = (FrameworkElement)e.AddedItems[0];
      var tag = (string)element.Tag;
      var theme = Enum.Parse<BaseTheme>(tag);

      var viewModel = (SettingsPageViewModel)DataContext;
      if (viewModel.ChangeBaseThemeCommand.CanExecute(theme))
        viewModel.ChangeBaseThemeCommand.Execute(theme);
    }
  }

}
