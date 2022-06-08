using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Sticky {

  // @TODO: The fact that there is even need for this
  // is so fucking dum...
  // https://www.codeproject.com/Articles/137209/Binding-and-styling-text-to-a-RichTextBox-in-WPF
  public class BindableRichTextBox : RichTextBox {
    public static readonly DependencyProperty DocumentProperty =
      DependencyProperty.Register("Document", typeof(FlowDocument),
      typeof(BindableRichTextBox), new FrameworkPropertyMetadata
      (null, new PropertyChangedCallback(OnDocumentChanged)));

    public new FlowDocument Document {
      get { return (FlowDocument)this.GetValue(DocumentProperty); }
      set { this.SetValue(DocumentProperty, value); }
    }

    public static void OnDocumentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) {
        var rtb = (RichTextBox)obj;
        rtb.Document = (FlowDocument)args.NewValue;
    }
  }

}
