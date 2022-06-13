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
      NoteRichTextBox.GotKeyboardFocus += OnGotKeyboardFocus;
      NoteRichTextBox.LostKeyboardFocus += OnLostKeyboardFocus;
      NoteRichTextBox.SelectionChanged += OnSelectionChanged;
      NoteRichTextBox.KeyUp += OnKeyUp;

      Native.ApplyRoundedWindowCorners(this);
      Show();
    }

    // Can't believe this...
    // https://stackoverflow.com/questions/5825575/detect-if-a-richtextbox-is-empty
    public bool IsRichTextBoxEmpty() {
      var box = NoteRichTextBox as RichTextBox;
      if (box == null) return true;

      var doc = box.Document;
      if (doc.Blocks.Count == 0) return true;

      var start = doc.ContentStart.GetNextInsertionPosition(LogicalDirection.Forward);
      var end = doc.ContentEnd.GetNextInsertionPosition(LogicalDirection.Backward);
      return start.CompareTo(end) == 0;
    }

    public void ShowOverlay() {
      Overlay.Visibility = Visibility.Visible;
    }

    private void OnToggleOverlay(object sender, RoutedEventArgs e) {
      var visibility = Overlay.Visibility;
      Overlay.Visibility = visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
    }

    private void OnOpenNotesList(object sender, RoutedEventArgs e) {
      System.Console.WriteLine("OnOpenNotesList");
    }

    private void OnDeleteNote(object sender, RoutedEventArgs e) {
      System.Console.WriteLine("OnDeleteNote");
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e) {
      var visibility = IsRichTextBoxEmpty() ? Visibility.Visible : Visibility.Hidden;
      Placeholder.Visibility = visibility;
    }

    private void RefreshToolbarButtons() {
      ToolbarButtonBold.IsChecked = NoteRichTextBox.IsBold();
      ToolbarButtonItalic.IsChecked = NoteRichTextBox.IsItalic();
      ToolbarButtonUnderline.IsChecked = NoteRichTextBox.IsUnderline();
      ToolbarButtonStrikethrough.IsChecked = NoteRichTextBox.IsStrikethrough();
      ToolbarButtonBullets.IsChecked = NoteRichTextBox.IsBullets();
    }

    private void OnSelectionChanged(object sender, RoutedEventArgs e) {
      RefreshToolbarButtons();
    }

    private void OnKeyUp(object sender, KeyEventArgs e) {
      // @TODO: Don't refresh unnecessarily.
      RefreshToolbarButtons();
    }

    private void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) {
      TitleBar.SlideIn();
    }

    private void OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) {
      TitleBar.SlideOut();
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e) {
      // @NOTE: This hack is here because:
      // Given: AllowsTransparency="True"
      // When: Window maximized
      // Then: Window size is greater than the monitor size... WTF?

      var thickness = (this.WindowState == WindowState.Maximized ? 8 : 0);
      this.BorderThickness = new System.Windows.Thickness(thickness);
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
      // @TODO: Remove from main window...
      base.OnClosing(e);
    }
  }
}
