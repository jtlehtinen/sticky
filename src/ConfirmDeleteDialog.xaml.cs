using System.Windows;

namespace Sticky {

  public partial class ConfirmDeleteDialog : Window {
    public ConfirmDeleteDialog() {
      InitializeComponent();
    }

    private void OnDelete(object sender, RoutedEventArgs e) {
      DialogResult = true;

    }

    private void OnKeep(object sender, RoutedEventArgs e) {
      DialogResult = false;
    }
  }

}
