using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Sticky {

  // @NOTE: Work-around for RichTextBox.Document property being unboundable.
  // https://www.codeproject.com/Articles/137209/Binding-and-styling-text-to-a-RichTextBox-in-WPF
  public class BindableRichTextBox : RichTextBox {
    public static readonly RoutedUICommand ToggleStrikethrough = new RoutedUICommand("ToggleStrikethrough", "ToggleStrikethrough", typeof(BindableRichTextBox));

    public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register("Document", typeof(FlowDocument), typeof(BindableRichTextBox), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnDocumentChanged)));

    public static void OnDocumentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
      var rtb = (RichTextBox)obj;
      rtb.Document = (FlowDocument)e.NewValue;
    }

    public BindableRichTextBox(): base() {
      // Register strikethrough command handler.
      CommandManager.RegisterClassCommandBinding(
        typeof(BindableRichTextBox),
        new CommandBinding(
          ToggleStrikethrough,
          OnToggleStrikethrough,
          CanToggleStrikethrough)
      );

      // Register strikethrough keyboard shortcut.
      CommandManager.RegisterClassInputBinding(
        typeof(BindableRichTextBox),
        new InputBinding(ToggleStrikethrough, new KeyGesture(Key.D, ModifierKeys.Control))
      );
    }

    public new FlowDocument Document {
      get { return (FlowDocument)this.GetValue(DocumentProperty); }
      set { this.SetValue(DocumentProperty, value); }
    }

    public bool IsBold() {
      return FontWeights.Bold.Equals(Selection.GetPropertyValue(Inline.FontWeightProperty));
    }

    public bool IsItalic() {
      return FontStyles.Italic.Equals(Selection.GetPropertyValue(Inline.FontStyleProperty));
    }

    public bool IsUnderline() {
      var textDecorations = Selection.GetPropertyValue(Inline.TextDecorationsProperty);
      if (textDecorations == DependencyProperty.UnsetValue) return false;

      return ContainsTextDecoration((TextDecorationCollection)textDecorations, TextDecorations.Underline[0]);
    }

    public bool IsStrikethrough() {
      var textDecorations = Selection.GetPropertyValue(Inline.TextDecorationsProperty);
      if (textDecorations == DependencyProperty.UnsetValue) return false;

      return ContainsTextDecoration((TextDecorationCollection)textDecorations, TextDecorations.Strikethrough[0]);
    }

    public bool IsSubscript() {
      var alignment = Selection.GetPropertyValue(Inline.BaselineAlignmentProperty);
      return BaselineAlignment.Subscript.Equals(alignment);
    }

    public bool IsSuperscript() {
      var alignment = Selection.GetPropertyValue(Inline.BaselineAlignmentProperty);
      return BaselineAlignment.Superscript.Equals(alignment);;
    }

    public bool IsBullets() {
      return IsSelectionBulletList(Selection);
    }

    private static void CanToggleStrikethrough(object sender, CanExecuteRoutedEventArgs e) {
      var rtb = (BindableRichTextBox)sender;
      e.CanExecute = !rtb.IsReadOnly;
    }

    private static void OnToggleStrikethrough(object target, ExecutedRoutedEventArgs args) {
      var rtb = target as BindableRichTextBox;
      if (rtb == null || rtb.IsReadOnly) return;

      var value = rtb.Selection.GetPropertyValue(Inline.TextDecorationsProperty);

      TextDecorationCollection? textDecorations = value != DependencyProperty.UnsetValue ? (TextDecorationCollection)value : null;

      TextDecorationCollection toggledTextDecorations;

      // @NOTE: Seems that roslyn warning analyzer issues tons of false positives.
      // Here just pulling out the if condition to a local variable causes
      // a false positive of CS8602: Dereference of a possibly null reference.
      if (textDecorations == null || textDecorations.Count == 0) {
        toggledTextDecorations = TextDecorations.Strikethrough;
      } else if (!textDecorations.TryRemove(TextDecorations.Strikethrough, out toggledTextDecorations)) {
        toggledTextDecorations.Add(TextDecorations.Strikethrough);
      }

      rtb.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, toggledTextDecorations);
    }

    private static bool ContainsTextDecoration(TextDecorationCollection? collection, TextDecoration needle) {
      if (collection == null) return false;

      var ValueEquals = (TextDecoration a, TextDecoration b) => {
        if (a == null && b == null) return true;
        if (a == null || b == null) return false;

        return (
          a.Location == b.Location &&
          a.PenOffset == b.PenOffset &&
          a.PenOffsetUnit == b.PenOffsetUnit &&
          a.PenThicknessUnit == b.PenThicknessUnit &&
          (a.Pen == null ? b.Pen == null : a.Pen.Equals( b.Pen)));
      };

      return collection.Any(d => ValueEquals(d, needle));
    }

    private static List? FindListAncestor(DependencyObject element) {
      while (element != null) {
        var list = element as List;
        if (list != null) return list;

        element = LogicalTreeHelper.GetParent(element);
      }
      return null;
    }

    private static bool IsSelectionBulletList(TextRange selection) {
      var list = FindListAncestor(selection.Start.Parent);
      return (list != null) && (list.MarkerStyle == TextMarkerStyle.Disc);
    }
  }

}
