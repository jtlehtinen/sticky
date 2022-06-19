using System.Windows;

namespace Sticky {

  public partial class NoteWindowTitleBar : TitleBarBase {
    public NoteWindowTitleBar() {
      InitializeComponent();
    }

    // @TODO: Use command instead and let the note window to handle it.
    private void OnToggleTopmost(object sender, RoutedEventArgs e) {
      var window = Window.GetWindow(this);
      window.Topmost = !window.Topmost;
    }

    // @TODO: Use command.
    private void OnMenu(object sender, RoutedEventArgs args) {
      var window = (NoteWindow)Window.GetWindow(this);
      window.ShowOverlay();
    }
  }

}
