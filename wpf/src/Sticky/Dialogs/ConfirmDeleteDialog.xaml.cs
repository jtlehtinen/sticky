using ModernWpf.Controls;

namespace Sticky {

  public partial class ConfirmDeleteDialog : ContentDialog {
    public ConfirmDeleteDialog() {
      InitializeComponent();
    }

    public bool ShouldNotAskAgain() {
      return DontAskAgain.IsChecked == true;
    }
  }

}
