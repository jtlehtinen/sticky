using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using ModernWpf.Controls;

namespace Sticky {
  /// <summary>
  /// Interaction logic for NoteWindow.xaml
  /// </summary>
  public partial class NoteWindow : Window {
    public NoteViewModel Note { get; private set; }

    public NoteWindow(NoteViewModel note) {
      Note = note;

      InitializeComponent();

      NoteRichTextBox.SelectionChanged += OnSelectionChanged;
      NoteRichTextBox.KeyUp += OnKeyUp;

      Native.ApplyRoundedWindowCorners(this);

      var document = XamlReader.Parse(note.Content) as FlowDocument;
      if (document != null) NoteRichTextBox.Document = document;

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

    private void SetTheme(string themeName) {
      var app = App.Current;
      if (app == null) return;

      var themeService = App.Current.Themes;
      if (themeService == null) return;

      var theme = themeService.GetTheme(themeName);
      if (theme == null) return;

      var currentTheme = Resources.MergedDictionaries.FirstOrDefault(d => themeService.IsTheme(d));
      if (currentTheme != null) Resources.MergedDictionaries.Remove(currentTheme);

      Resources.MergedDictionaries.Add(theme);
    }

    private void ChangeNoteThemeExecuted(object sender, ExecutedRoutedEventArgs e) {
      var themeName = (string)e.Parameter;
      SetTheme(themeName);
    }

    private void ToggleTopmostExecuted(object sender, ExecutedRoutedEventArgs e) {
      Topmost = !Topmost;
    }

    private void ShowMenuExecuted(object sender, ExecutedRoutedEventArgs e) {
      ShowOverlay();
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

    private async void OnDeleteNote(object sender, RoutedEventArgs e) {
      System.Console.WriteLine("OnDeleteNote");

      ConfirmDeleteDialog dialog = new ConfirmDeleteDialog();
      var result = await dialog.ShowAsync();

      var doDelete = (result == ContentDialogResult.Primary);
      if (doDelete) {
        // @TODO: Delete the note
      }
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
      // @NOTE: Hack! When the window is maximized the window size ends
      // up being greater than the monitor size.
      var thickness = (this.WindowState == WindowState.Maximized ? 8 : 0);
      this.BorderThickness = new System.Windows.Thickness(thickness);
    }
  }
}
