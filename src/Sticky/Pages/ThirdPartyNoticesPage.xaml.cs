using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Sticky.ViewModels;

namespace Sticky {

  public partial class ThirdPartyNoticesPage : UserControl {
    private DragBehavior _drag;

    public ThirdPartyNoticesPage() {
      InitializeComponent();

      _drag = new DragBehavior(PartTitleBar);
    }

    private void OnBack(object sender, RoutedEventArgs e) {
      App.Current.MainWindow.Navigate(PageType.Settings);
    }

    private void OnOpenBrowser(object sender, RequestNavigateEventArgs e) {
      var info = new ProcessStartInfo(e.Uri.AbsoluteUri);
      info.UseShellExecute = true;

      Process.Start(info);
      e.Handled = true;
    }
  }

}
