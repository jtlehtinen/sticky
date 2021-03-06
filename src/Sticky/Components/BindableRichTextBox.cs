using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;

namespace Sticky {

  // https://stackoverflow.com/questions/343468/richtextbox-wpf-binding
  public class RichTextBoxHelper : DependencyObject {
    public const string EMPTY_FLOW_DOCUMENT_XAML = "<FlowDocument PagePadding=\"5,0,5,0\" AllowDrop=\"True\" NumberSubstitution.CultureSource=\"User\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Paragraph /></FlowDocument>";

    public static string GetDocumentXaml(DependencyObject d) {
      return (string)d.GetValue(DocumentXamlProperty);
    }

    public static void SetDocumentXaml(DependencyObject d, string value) {
      d.SetValue(DocumentXamlProperty, value);
    }

    public static readonly DependencyProperty DocumentXamlProperty = DependencyProperty.RegisterAttached("DocumentXaml", typeof(string), typeof(RichTextBoxHelper), new FrameworkPropertyMetadata {
      BindsTwoWayByDefault = true,
      PropertyChangedCallback = OnDocumentXamlChanged,
    });

    public static bool IsFlowDocumentEmpty(FlowDocument doc) {
      // https://stackoverflow.com/questions/5825575/detect-if-a-richtextbox-is-empty
      if (doc.Blocks.Count == 0) return true;

      var start = doc.ContentStart.GetNextInsertionPosition(LogicalDirection.Forward);
      var end = doc.ContentEnd.GetNextInsertionPosition(LogicalDirection.Backward);
      return start.CompareTo(end) == 0;
    }

    private static void OnDocumentXamlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
      var rtb = (BindableRichTextBox)d;

      var currentValue = SerializeToXaml(rtb.Document);
      var newValue = GetDocumentXaml(rtb);
      if (currentValue == newValue) return;

      var xaml = string.IsNullOrEmpty(newValue) ? EMPTY_FLOW_DOCUMENT_XAML : newValue;
      rtb.Document = DeserializeFromXaml(xaml);

      rtb.TextChanged -= TextChangedEventHandler;
      rtb.TextChanged += TextChangedEventHandler;
    }

    private static void TextChangedEventHandler(object sender, TextChangedEventArgs e) {
      var rtb = (BindableRichTextBox)sender;
      var xaml = SerializeToXaml(rtb.Document);
      SetDocumentXaml(rtb, xaml);
    }

    public static FlowDocument DeserializeFromXaml(string xaml) {
      var stream = new MemoryStream(Encoding.UTF8.GetBytes(xaml));
      return XamlReader.Load(stream) as FlowDocument;
    }

    public static string SerializeToXaml(FlowDocument doc) {
      return XamlWriter.Save(doc);
    }
  }

  public class BindableRichTextBox : RichTextBox {
    public static readonly RoutedUICommand ToggleStrikethrough = new RoutedUICommand("ToggleStrikethrough", "ToggleStrikethrough", typeof(BindableRichTextBox));

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
        new InputBinding(ToggleStrikethrough, new KeyGesture(Key.T, ModifierKeys.Control))
      );
    }

    public bool IsEmpty {
      get { return RichTextBoxHelper.IsFlowDocumentEmpty(Document); }
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
