using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Sticky {

  public partial class NoteControl : UserControl {
    public NoteControl() {
      DataContextChanged += (sender, e) => {
        RefreshTheme();

        // @TODO: Remove event handler.
        var note = (NoteViewModel)DataContext;
        note.PropertyChanged += (s, e) => {
          if (e.PropertyName == "Theme") {
            RefreshTheme();
          }
        };
      };

      InitializeComponent();
      // @TODO: Handle note theme changes.
    }

    private void RefreshTheme() {
      var note = DataContext as NoteViewModel;
      if (note == null) return;

      var theme = App.Current.Themes.GetTheme(note.Theme);
      if (theme == null) return;

      Resources.Clear();
      Resources.MergedDictionaries.Add(theme);
    }

    private void OnOpenContextMenu(object sender, ExecutedRoutedEventArgs e) {
      // @TODO: Context menu won't work properly without focus?
      NoteRichTextBox.Focus();

      var cm = ContextMenuService.GetContextMenu(NoteRichTextBox);
      cm.PlacementTarget = NoteRichTextBox;
      cm.Placement = PlacementMode.Center;
      cm.IsOpen = true;
    }
  }

}
