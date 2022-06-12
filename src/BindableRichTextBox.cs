using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Sticky {


  // @TODO: The fact that there is even need for this
  // is so fucking dum...
  // https://www.codeproject.com/Articles/137209/Binding-and-styling-text-to-a-RichTextBox-in-WPF
  public class BindableRichTextBox : RichTextBox {
    public static readonly RoutedUICommand ToggleStrikethrough = new RoutedUICommand("ToggleStrikethrough", "ToggleStrikethrough", typeof(BindableRichTextBox));

    public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register("Document", typeof(FlowDocument), typeof(BindableRichTextBox), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnDocumentChanged)));
    public static readonly DependencyProperty IsItalicProperty = DependencyProperty.Register("IsItalic", typeof(bool), typeof(BindableRichTextBox));
    public static readonly DependencyProperty IsBoldProperty = DependencyProperty.Register("IsBold", typeof(bool), typeof(BindableRichTextBox));
    public static readonly DependencyProperty IsUnderlineProperty = DependencyProperty.Register("IsUnderline", typeof(bool), typeof(BindableRichTextBox));
    public static readonly DependencyProperty IsStrikethroughProperty = DependencyProperty.Register("IsStrikethrough", typeof(bool), typeof(BindableRichTextBox));
    public static readonly DependencyProperty IsSubscriptProperty = DependencyProperty.Register("IsSubscript", typeof(bool), typeof(BindableRichTextBox));
    public static readonly DependencyProperty IsSuperscriptProperty = DependencyProperty.Register("IsSuperscript", typeof(bool), typeof(BindableRichTextBox));
    public static readonly DependencyProperty IsBulletsProperty = DependencyProperty.Register("IsBullets", typeof(bool), typeof(BindableRichTextBox));

    public static void OnDocumentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
      var rtb = (RichTextBox)obj;
      rtb.Document = (FlowDocument)e.NewValue;
    }

    public BindableRichTextBox(): base() {
      // Register strikethrough command handler and keyboard shortcut.
      CommandManager.RegisterClassCommandBinding(
        typeof(BindableRichTextBox),
        new CommandBinding(
          ToggleStrikethrough,
          new ExecutedRoutedEventHandler(OnToggleStrikethrough),
          new CanExecuteRoutedEventHandler(CanToggleStrikethrough))
      );

      CommandManager.RegisterClassInputBinding(
        typeof(BindableRichTextBox),
        new InputBinding(ToggleStrikethrough, new KeyGesture(Key.D, ModifierKeys.Control))
      );
    }

    public new FlowDocument Document {
      get { return (FlowDocument)this.GetValue(DocumentProperty); }
      set { this.SetValue(DocumentProperty, value); }
    }

    // @TODO: This mess of binding to a value computed
    // from other dependency properties feels so ugly.
    // Fix this shit by figuring out something more sane.
    protected override void OnKeyUp(KeyEventArgs e) {
      base.OnKeyUp(e);
      //var ctrl = e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control);
      UpdateComputedDependencyProperties();
    }

    protected override void OnSelectionChanged(RoutedEventArgs e) {
      base.OnSelectionChanged(e);
      UpdateComputedDependencyProperties();
    }

    private List? FindListAncestor(DependencyObject element) {
      while (element != null) {
        var list = element as List;
        if (list != null) return list;

        element = LogicalTreeHelper.GetParent(element);
      }
      return null;
    }

    private bool IsSelectionBulletList() {
      var list = FindListAncestor(Selection.Start.Parent);
      if (list == null) return false;

      return (list.MarkerStyle == TextMarkerStyle.Disc);
    }

    private void UpdateComputedDependencyProperties() {
      var fontWeight = Selection.GetPropertyValue(Inline.FontWeightProperty);
      IsBold = fontWeight.Equals(FontWeights.Bold);

      var fontStyle = Selection.GetPropertyValue(Run.FontStyleProperty);
      IsItalic = fontStyle.Equals(FontStyles.Italic);

      var textDecorations = Selection.GetPropertyValue(Inline.TextDecorationsProperty);
      IsUnderline = textDecorations.Equals(TextDecorations.Underline);
      IsStrikethrough = textDecorations.Equals(TextDecorations.Strikethrough);

      var baselineAlignment = Selection.GetPropertyValue(Inline.BaselineAlignmentProperty);
      IsSubscript = baselineAlignment.Equals(BaselineAlignment.Subscript);
      IsSuperscript = baselineAlignment.Equals(BaselineAlignment.Superscript);

      IsBullets = IsSelectionBulletList();
    }

    public bool IsBold {
      get { return (bool)GetValue(IsBoldProperty); }
      private set { SetValue(IsBoldProperty, value); }
    }

    public bool IsItalic {
      get { return (bool)GetValue(IsItalicProperty); }
      private set { SetValue(IsItalicProperty, value); }
    }

    public bool IsUnderline {
      get { return (bool)GetValue(IsUnderlineProperty); }
      private set { SetValue(IsUnderlineProperty, value); }
    }

    public bool IsStrikethrough {
      get { return (bool)GetValue(IsStrikethroughProperty); }
      private set { SetValue(IsStrikethroughProperty, value); }
    }

    public bool IsSubscript {
      get { return (bool)GetValue(IsSubscriptProperty); }
      private set { SetValue(IsSubscriptProperty, value); }
    }

    public bool IsSuperscript {
      get { return (bool)GetValue(IsSuperscriptProperty); }
      private set { SetValue(IsSuperscriptProperty, value); }
    }

    public bool IsBullets {
      get { return (bool)GetValue(IsBulletsProperty); }
      private set { SetValue(IsBulletsProperty, value); }
    }

    private static void CanToggleStrikethrough(object sender, CanExecuteRoutedEventArgs e) {
      var rtb = (BindableRichTextBox)sender;
      e.CanExecute = !rtb.IsReadOnly;
    }

    private static void OnToggleStrikethrough(object target, ExecutedRoutedEventArgs args) {
      var rtb = target as BindableRichTextBox;
      if (rtb == null || rtb.IsReadOnly) return;

      object value = rtb.Selection.GetPropertyValue(Inline.TextDecorationsProperty);

      TextDecorationCollection? textDecorations = value != DependencyProperty.UnsetValue ? (TextDecorationCollection)value : null;

      TextDecorationCollection toggledTextDecorations;

      var noDecorations = textDecorations == null || textDecorations.Count == 0;
      if (noDecorations) {
        toggledTextDecorations = TextDecorations.Strikethrough;
      } else if (!textDecorations.TryRemove(TextDecorations.Strikethrough, out toggledTextDecorations)) {
        toggledTextDecorations.Add(TextDecorations.Strikethrough);
      }

      rtb.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, toggledTextDecorations);
    }
  }

}
