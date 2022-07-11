using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using Sticky.Helpers;
using Sticky.ViewModels;

namespace Sticky {

  public partial class NoteWindow : Window {
    private DragBehavior _drag;
    private FixMaximizedWindowSizeBehavior _sizeFix;

    public NoteWindow(NoteWindowViewModel vm) {
      DataContext = vm;
      Closing += (sender, e) => SaveWindowPlacement();

      InitializeComponent();
      Native.ApplyRoundedWindowCorners(this);

      LoadWindowPlacement();

      _drag = new DragBehavior(TitleBar);
      _sizeFix = new FixMaximizedWindowSizeBehavior(this);

      vm.ShowOverlayRequested += () => ShowOverlay();
      vm.HideOverlayRequested += () => HideOverlay();

      vm.PropertyChanged += (s, e) => {
        if (e.PropertyName == "Theme") SetTheme(vm.Theme);
      };

      NoteRichTextBox.IsKeyboardFocusedChanged += (sender, e) => RefreshToolbarVisibility(this.RenderSize);
      NoteRichTextBox.SelectionChanged += (sender, e) => RefreshToolbarButtons();
      NoteRichTextBox.KeyUp += (sender, e) => RefreshToolbarButtons();

      Overlay.IsKeyboardFocusWithinChanged += (sender, e) => {
        var focused = (bool)e.NewValue;
        if (!focused) HideOverlay();
      };

      SizeChanged += (sender, e) => RefreshToolbarVisibility(e.NewSize);

      SetTheme(vm.Theme);
    }

    public NoteWindowViewModel GetViewModel() {
      return DataContext as NoteWindowViewModel;
    }

    public bool ContainsNote(int noteId) {
      var vm = GetViewModel();
      if (vm == null) return false;

      return vm.Id == noteId;
    }

    public void TakeKeyboardFocus() {
      NoteRichTextBox.CaretPosition = NoteRichTextBox.Document.ContentEnd;
      NoteRichTextBox.Focus();
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
    }

    private void ShowOverlay() {
      Overlay.Visibility = Visibility.Visible;
      Keyboard.Focus(FirstColorTheme);
    }

    private void HideOverlay() {
      Overlay.Visibility = Visibility.Collapsed;
    }

    private void RefreshToolbarVisibility(Size noteWindowSize) {
      var hasKeyboardFocus = IsKeyboardFocusWithin;

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
      var vm = GetViewModel();
      vm.Theme = (string)e.Parameter;
      HideOverlay();
    }

    private void OnHideOverlay(object sender, RoutedEventArgs e) {
      HideOverlay();
    }

    private void OnMouseWheel(object sender, MouseWheelEventArgs e) {
      if (Keyboard.Modifiers != ModifierKeys.Control)
        return;

      // @NOTE: Delta should be 120 or -120.
      var command = e.Delta > 0 ? EditingCommands.IncreaseFontSize : EditingCommands.DecreaseFontSize;
      command.Execute(null, (RichTextBox)sender);
    }

    private void LoadWindowPlacement() {
      var vm = GetViewModel();
      if (vm == null) return;

      if (string.IsNullOrWhiteSpace(vm.WindowPosition)) return;

      var handle = new WindowInteropHelper(this).Handle;
      var placement = WindowHelper.DeserializePlacementOrDefault(handle, vm.WindowPosition);

      Native.SetWindowPlacement(handle, ref placement);
    }

    private void SaveWindowPlacement() {
      var vm = GetViewModel();
      if (vm == null) return;

      var handle = new WindowInteropHelper(this).Handle;
      vm.WindowPosition = WindowHelper.SerializePlacement(handle);
    }
  }

}
