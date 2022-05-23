using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Sticky {
  /// <summary>
  /// Interaction logic for NoteWindow.xaml
  /// </summary>
  public partial class NoteWindow : Window {

    public NoteWindow() {
      InitializeComponent();
      Native.ApplyRoundedWindowCorners(this);
      this.Show();
      this.NoteInput.GotKeyboardFocus += OnGotKeyboardFocus;
      this.NoteInput.LostKeyboardFocus += OnLostKeyboardFocus;
    }

    // Can't believe this...
    // https://stackoverflow.com/questions/5825575/detect-if-a-richtextbox-is-empty
    public bool IsRichTextBoxEmpty() {
      var box = this.NoteInput as RichTextBox;
      if (box == null) return true;

      if (box.Document.Blocks.Count == 0) return true;
      var start = box.Document.ContentStart.GetNextInsertionPosition(LogicalDirection.Forward);
      var end = box.Document.ContentEnd.GetNextInsertionPosition(LogicalDirection.Backward);
      return start.CompareTo(end) == 0;
    }

    private void OnTextChanged(object sender, TextChangedEventArgs args) {
      var visibility = IsRichTextBoxEmpty() ? Visibility.Visible : Visibility.Hidden;
      this.Placeholder.Visibility = visibility;
    }

    private void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs args) {
      this.TitleBar.SlideIn();
    }

    private void OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs args) {
      this.TitleBar.SlideOut();
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs args) {
      // @NOTE: This hack is here because:
      // Given: AllowsTransparency="True"
      // When: Window maximized
      // Then: Window size is greater than the monitor size... WTF?

      var thickness = (this.WindowState == WindowState.Maximized ? 8 : 0);
      this.BorderThickness = new System.Windows.Thickness(thickness);
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs args) {
      // @TODO: Remove from main window...
      base.OnClosing(args);
    }
  }
}
