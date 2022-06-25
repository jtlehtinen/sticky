using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sticky {
  /// <summary>
  /// Interaction logic for NoteWindow.xaml
  /// </summary>
  public partial class NoteWindow : Window {
    public NoteWindow(NoteViewModel note) {
      InitializeComponent();
      Native.ApplyRoundedWindowCorners(this);

      DataContext = note;

      // @TODO: Remove event handler.
      note.PropertyChanged += (s, e) => {
        if (e.PropertyName == "Theme") SetTheme(note.Theme);
      };

      NoteRichTextBox.SelectionChanged += OnSelectionChanged;
      NoteRichTextBox.KeyUp += OnKeyUp;

      LostKeyboardFocus += (sender, e) => HideOverlay();
      LostFocus += (sender, e) => HideOverlay();

      SetTheme(note.Theme);
    }

    private void SetTheme(string themeName) {
      var app = App.Current;
      if (app == null) return;

      var themes = App.Current.Themes;
      if (themes == null) return;

      var theme = themes.GetTheme(themeName);
      if (theme == null) return;

      var currentTheme = Resources.MergedDictionaries.FirstOrDefault(d => themes.IsTheme(d));
      if (currentTheme == theme) return;

      // @NOTE: Add the new one before removing the old one to avoid
      // missing key warnings. There is no atomic replace op...
      Resources.MergedDictionaries.Add(theme);
      if (currentTheme != null) Resources.MergedDictionaries.Remove(currentTheme);

      switch (themeName) {
        case "Theme.Yellow": RadioButtonThemeYellow.IsChecked = true; break;
        case "Theme.Green": RadioButtonThemeGreen.IsChecked = true; break;
        case "Theme.Pink": RadioButtonThemePink.IsChecked = true; break;
        case "Theme.Purple": RadioButtonThemePurple.IsChecked = true; break;
        case "Theme.Blue": RadioButtonThemeBlue.IsChecked = true; break;
        case "Theme.Gray": RadioButtonThemeGray.IsChecked = true; break;
        case "Theme.Charcoal": RadioButtonThemeCharcoal.IsChecked = true; break;
      }
    }

    private void ShowOverlay() {
      Overlay.Visibility = Visibility.Visible;
    }

    private void HideOverlay() {
      Overlay.Visibility = Visibility.Collapsed;
    }

    private void RefreshToolbarButtons() {
      ToolbarButtonBold.IsChecked = NoteRichTextBox.IsBold();
      ToolbarButtonItalic.IsChecked = NoteRichTextBox.IsItalic();
      ToolbarButtonUnderline.IsChecked = NoteRichTextBox.IsUnderline();
      ToolbarButtonStrikethrough.IsChecked = NoteRichTextBox.IsStrikethrough();
      ToolbarButtonBullets.IsChecked = NoteRichTextBox.IsBullets();
    }

    private void ChangeNoteThemeExecuted(object sender, ExecutedRoutedEventArgs e) {
      var note = (NoteViewModel)DataContext;
      note.Theme = (string)e.Parameter;
      HideOverlay();
    }

    private void ToggleTopmostExecuted(object sender, ExecutedRoutedEventArgs e) {
      Topmost = !Topmost;
    }

    private void OnHideOverlay(object sender, RoutedEventArgs e) {
      HideOverlay();
    }

    private void ShowMenuExecuted(object sender, ExecutedRoutedEventArgs e) {
      ShowOverlay();
    }

    private void OnSelectionChanged(object sender, RoutedEventArgs e) {
      RefreshToolbarButtons();
    }

    private void OnKeyUp(object sender, KeyEventArgs e) {
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
