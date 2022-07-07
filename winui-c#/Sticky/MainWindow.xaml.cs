using Microsoft.UI.Xaml;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using Sticky.Helpers;

namespace Sticky;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window {
  private AppWindow _appWindow;
  private OverlappedPresenter _presenter;

  public MainWindow() {
    GetAppWindowAndPresenter();
    _presenter.IsResizable = false;

    InitializeComponent();

    _appWindow.Resize(new Windows.Graphics.SizeInt32 { Width = 320, Height = 500 });

    Title = "AppDisplayName".GetLocalized();

    ExtendsContentIntoTitleBar = true;
    SetTitleBar(AppTitleBar);
  }


  public void GetAppWindowAndPresenter() {
	  var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
	  var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
	  _appWindow = AppWindow.GetFromWindowId(windowId);
	  _presenter = _appWindow.Presenter as OverlappedPresenter;
  }
}
