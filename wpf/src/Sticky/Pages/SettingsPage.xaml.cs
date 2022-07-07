using System;
using System.Windows;
using System.Windows.Controls;
using Sticky.ViewModels;

namespace Sticky {

  public partial class SettingsPage : UserControl {
    private DragBehavior _drag;

    public SettingsPage(SettingsPageViewModel viewModel) {
      DataContext = viewModel;
      InitializeComponent();

      _drag = new DragBehavior(PartTitleBar);
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
