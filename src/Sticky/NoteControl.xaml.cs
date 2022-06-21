using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Sticky {

  public partial class NoteControl : UserControl {
    public NoteControl() {
      DataContextChanged += OnDataContextChanged;
      InitializeComponent();
      // @TODO: Handle note theme changes.
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
      var note = DataContext as NoteViewModel;
      if (note == null) return;

      var theme = App.Current.Themes.GetTheme(note.Theme);
      if (theme == null) return;

      Resources.Clear();
      Resources.MergedDictionaries.Add(theme);
    }

    private void OnOpenContextMenu(object sender, ExecutedRoutedEventArgs e) {
      System.Console.WriteLine("OnOpenContextMenu");

      // @TODO: Something weird happens here. The bindings used
      // in the context menu do not work before the note control
      // is left clicked once. That is, at least one of the note
      // controls is clicked. After that the bindings work for
      // all note controls... ???
      var cm = ContextMenuService.GetContextMenu(NoteRichTextBox);
      cm.PlacementTarget = NoteRichTextBox;
      cm.Placement = PlacementMode.Center;
      cm.IsOpen = true;
    }
  }

}
