using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Sticky.ViewModels;

namespace Sticky {
  /// <summary>
  /// Interaction logic for NoteWindow.xaml
  /// </summary>
  public partial class NoteWindow : Window {
    private DragBehavior _drag;

    public NoteWindow(NoteWindowViewModel vm) {
      DataContext = vm;

      InitializeComponent();
      Native.ApplyRoundedWindowCorners(this);

      _drag = new DragBehavior(TitleBar);

      // @TODO: Remove event handler.
      vm.PropertyChanged += (s, e) => {
        if (e.PropertyName == "Theme") SetTheme(vm.Theme);
      };

      // @TODO: Move to XAML?
      NoteRichTextBox.IsKeyboardFocusedChanged += (sender, e) => RefreshToolbarVisibility(this.RenderSize);
      NoteRichTextBox.SelectionChanged += OnSelectionChanged;
      NoteRichTextBox.KeyUp += OnKeyUp;

      LostKeyboardFocus += (sender, e) => HideOverlay();
      LostFocus += (sender, e) => HideOverlay();

      SetTheme(vm.Theme);
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

      /*
      switch (themeName) {
        case "Theme.Yellow": RadioButtonThemeYellow.IsChecked = true; break;
        case "Theme.Green": RadioButtonThemeGreen.IsChecked = true; break;
        case "Theme.Pink": RadioButtonThemePink.IsChecked = true; break;
        case "Theme.Purple": RadioButtonThemePurple.IsChecked = true; break;
        case "Theme.Blue": RadioButtonThemeBlue.IsChecked = true; break;
        case "Theme.Gray": RadioButtonThemeGray.IsChecked = true; break;
        case "Theme.Charcoal": RadioButtonThemeCharcoal.IsChecked = true; break;
      }
      */
    }

    private void ShowOverlay() {
      Overlay.Visibility = Visibility.Visible;
    }

    private void HideOverlay() {
      Overlay.Visibility = Visibility.Collapsed;
    }

    private void RefreshToolbarVisibility(Size noteWindowSize) {
      var hasKeyboardFocus = NoteRichTextBox.IsKeyboardFocusWithin;

      var show = hasKeyboardFocus && noteWindowSize.Width >= 340 && noteWindowSize.Height >= 150;
      var visibility = show ? Visibility.Visible : Visibility.Collapsed;

      if (Toolbar.Visibility != visibility) Toolbar.Visibility = visibility;
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

    private void OnMouseWheel(object sender, MouseWheelEventArgs e) {
      if (Keyboard.Modifiers != ModifierKeys.Control)
        return;

      // @NOTE: Delta should be always 120 or -120.
      var command = e.Delta > 0 ? EditingCommands.IncreaseFontSize : EditingCommands.DecreaseFontSize;
      command.Execute(null, (RichTextBox)sender);
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e) {
      // @NOTE: Hack! When the window is maximized the window size ends
      // up being greater than the monitor size.
      var thickness = (this.WindowState == WindowState.Maximized ? 8 : 0);
      this.BorderThickness = new System.Windows.Thickness(thickness);

      RefreshToolbarVisibility(e.NewSize);
    }
  }
}
