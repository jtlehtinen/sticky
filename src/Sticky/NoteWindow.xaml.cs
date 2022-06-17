using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;

using System.Windows.Controls.Primitives;

namespace Sticky {
  /// <summary>
  /// Interaction logic for NoteWindow.xaml
  /// </summary>
  public partial class NoteWindow : Window {
    public static RoutedCommand ChangeThemeCommand = new RoutedCommand();

    private int noteId;

    public NoteWindow() {
      InitializeComponent();

      CommandBindings.Add(new CommandBinding(ChangeThemeCommand, ExecutedChangeThemeCommand));

      NoteRichTextBox.SelectionChanged += OnSelectionChanged;
      NoteRichTextBox.KeyUp += OnKeyUp;

      SetTheme("Theme.Green");
      RadioButtonThemeGreen.IsChecked = true;

      Native.ApplyRoundedWindowCorners(this);
      Show();
    }

    // @TODO: ...
    public NoteWindow(Note note) : this() {
      if (note.Content != null) {
        noteId = note.Id;
        var document = XamlReader.Parse(note.Content) as FlowDocument;
        if (document != null) NoteRichTextBox.Document = document;

        if (note.Theme != null) {
          SetTheme(note.Theme);

          switch (note.Theme) {
            case "Theme.Yellow": RadioButtonThemeYellow.IsChecked = true; break;
            case "Theme.Green": RadioButtonThemeGreen.IsChecked = true; break;
            case "Theme.Pink": RadioButtonThemePink.IsChecked = true; break;
            case "Theme.Purple": RadioButtonThemePurple.IsChecked = true; break;
            case "Theme.Blue": RadioButtonThemeBlue.IsChecked = true; break;
            case "Theme.Gray": RadioButtonThemeGray.IsChecked = true; break;
            case "Theme.Charcoal": RadioButtonThemeCharcoal.IsChecked = true; break;
          }
        }
      }
    }

    private void SetTheme(string themeName) {
      var app = App.Current;
      if (app == null) return;

      var themeService = app.Services.GetService<ThemeService>();
      if (themeService == null) return;

      var theme = themeService.GetTheme(themeName);
      if (theme == null) return;

      var currentTheme = Resources.MergedDictionaries.FirstOrDefault(d => themeService.IsTheme(d));
      if (currentTheme != null) Resources.MergedDictionaries.Remove(currentTheme);

      Resources.MergedDictionaries.Add(theme);
    }

    private void ExecutedChangeThemeCommand(object sender, ExecutedRoutedEventArgs e) {
      var themeName = (string)e.Parameter;
      SetTheme(themeName);
    }

    // Can't believe this...
    // https://stackoverflow.com/questions/5825575/detect-if-a-richtextbox-is-empty
    // @TODO: Is property and bind Placeholder visiblity on it...
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

    private void HideOverlay(object sender, RoutedEventArgs e) {
      Overlay.Visibility = Visibility.Collapsed;
    }

    private void OnOpenNotesList(object sender, RoutedEventArgs e) {
      System.Console.WriteLine("OnOpenNotesList");
    }

    private void OnDeleteNote(object sender, RoutedEventArgs e) {
      System.Console.WriteLine("OnDeleteNote");

      var confirm = new ConfirmDeleteDialog { Owner = this };
      confirm.ShowDialog();

      if (confirm.DialogResult == true) {
        // @TODO: Delete note
      }
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
