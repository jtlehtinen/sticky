using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sticky {

  public partial class NoteControl : UserControl {
    public NoteControl() {
      InitializeComponent();
      DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
      var note = DataContext as NoteViewModel;
      if (note == null) return;

      var theme = App.Current.Themes.GetTheme(note.Theme);
      if (theme == null) return;

      Resources.Clear();
      Resources.MergedDictionaries.Add(theme);
    }
  }

}
