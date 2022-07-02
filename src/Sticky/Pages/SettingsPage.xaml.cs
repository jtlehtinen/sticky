using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using Sticky.ViewModels;

namespace Sticky {

  public partial class SettingsPage : UserControl {
    private DragBehavior _drag;

    public SettingsPage(SettingsPageViewModel vm) {
      DataContext = vm;
      InitializeComponent();
      _drag = new DragBehavior(this);
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
