using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Sticky.ViewModels;

namespace Sticky {

  public partial class NoteControl : UserControl {
    public NoteControl() {
      DataContextChanged += OnDataContextChanged;
      InitializeComponent();
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
      var vm = (NoteViewModel)DataContext;
      vm.OpenContextMenuRequested += () => OpenContextMenu();

      RefreshTheme(vm);
    }

    private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e) {
      OpenContextMenu(PlacementMode.MousePoint);
    }

    private void RefreshTheme(NoteViewModel vm) {
      var theme = App.Current.Themes.GetTheme(vm.Theme);
      if (theme == null) return;

      Resources.Clear();
      Resources.MergedDictionaries.Add(theme);
    }

    private void OpenContextMenu(PlacementMode placement = PlacementMode.Right) {
      var width = this.Width;
      var height = this.Height;

      var cm = ContextMenuService.GetContextMenu(NoteRichTextBox);
      cm.PlacementTarget = NoteRichTextBox;
      cm.Placement = placement;
      cm.IsOpen = true;
    }
  }

}
