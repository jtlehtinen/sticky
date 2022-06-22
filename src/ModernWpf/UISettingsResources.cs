using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Win32;
#if false // @TODO
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
#endif

namespace ModernWpf {
  internal class UISettingsResources : ResourceDictionary {
    private const string UniversalApiContractName = "Windows.Foundation.UniversalApiContract";
    private const string AutoHideScrollBarsKey = "AutoHideScrollBars";

    private readonly Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;

#if false // @TODO
    private UISettings _uiSettings;
#endif

    public UISettingsResources() {
      if (DesignMode.DesignModeEnabled) {
        return;
      }

      if (OSVersionHelper.IsWindows10OrGreater) {
        Initialize();
      }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void Initialize() {
#if false // @TODO
      _uiSettings = new UISettings();

      if (ApiInformation.IsApiContractPresent(UniversalApiContractName, 4)) {
        InitializeForContract4();
      }

      if (ApiInformation.IsApiContractPresent(UniversalApiContractName, 8)) {
        InitializeForContract8();
      }
#endif
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void InitializeForContract4() {
#if false // @TODO
      _uiSettings.AdvancedEffectsEnabledChanged += (sender, args) => {
        _dispatcher.BeginInvoke(ApplyAdvancedEffectsEnabled);
      };

      if (PackagedAppHelper.IsPackagedApp) {
        SystemEvents.UserPreferenceChanged += (sender, args) => {
          if (args.Category == UserPreferenceCategory.General) {
            ApplyAdvancedEffectsEnabled();
          }
        };
      }

      ApplyAdvancedEffectsEnabled();
#endif
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void InitializeForContract8() {
#if false // @TODO
      _uiSettings.AutoHideScrollBarsChanged += (sender, args) => {
        _dispatcher.BeginInvoke(ApplyAutoHideScrollBars);
      };
      ApplyAutoHideScrollBars();
#endif
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void ApplyAdvancedEffectsEnabled() {
#if false // @TODO
      var key = SystemParameters.DropShadowKey;
      if (_uiSettings.AdvancedEffectsEnabled) {
        Remove(key);
      } else {
        this[key] = false;
      }
#endif
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void ApplyAutoHideScrollBars() {
#if false // @TODO
      this[AutoHideScrollBarsKey] = _uiSettings.AutoHideScrollBars;
#endif
    }
  }
}
