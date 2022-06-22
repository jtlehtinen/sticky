using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace Sticky {

  public partial class SettingsPage : UserControl {
    public SettingsPage() {
      InitializeComponent();
    }

    private void OnExportNotes(object sender, RoutedEventArgs e) {
      System.Console.WriteLine("OnExportNotes");

      var dialog = new SaveFileDialog();
      dialog.FileName = "sticky-notes";
      dialog.DefaultExt = ".json";
      dialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
      dialog.ShowDialog();
      if (dialog.FileName == "") {
        return;
      }

      // @TODO: Export notes...
    }
  }

}
