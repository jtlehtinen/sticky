using System;
using System.Windows;
using System.Windows.Controls;
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
  }

}
