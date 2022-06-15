using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Sticky {
  /// <summary>
  /// Interaction logic for NoteWindow.xaml
  /// </summary>
  public partial class NoteWindow : Window {
    public static RoutedCommand ChangeThemeCommand = new RoutedCommand();

    public NoteWindow() {
      InitializeComponent();

      CommandBindings.Add(new CommandBinding(ChangeThemeCommand, ExecutedChangeThemeCommand));

      NoteRichTextBox.SelectionChanged += OnSelectionChanged;
      NoteRichTextBox.KeyUp += OnKeyUp;

      SetTheme("Theme.Green");
      Native.ApplyRoundedWindowCorners(this);
      Show();
    }

    private ResourceDictionary? FindThemeFromResources() {
      foreach (var dic in Resources.MergedDictionaries) {
        if (dic.Contains(App.THEME_MARKER_KEY)) return dic;
      }
      return null;
    }

    private void SetTheme(string themeName) {
      var app = Application.Current as App;
      if (app == null) return;

      if (!app.Properties.Contains(themeName)) return;

      var theme = app.Properties[themeName] as ResourceDictionary;
      if (theme == null) return;

      var currentTheme = FindThemeFromResources();
      if (currentTheme != null) Resources.MergedDictionaries.Remove(currentTheme);

      Resources.MergedDictionaries.Add(theme);
    }

    private void ExecutedChangeThemeCommand(object sender, ExecutedRoutedEventArgs e) {
      var parameter = (string)e.Parameter;
      SetTheme(parameter);
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

    private void OnToggleOverlay(object sender, RoutedEventArgs e) {
      var visibility = Overlay.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
      Overlay.Visibility = visibility;
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
