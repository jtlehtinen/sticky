using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Win32;
using ModernWpf.Media.ColorPalette;

#if false // @TODO
using Windows.UI.ViewManagement;
#endif

namespace ModernWpf {

  // https://docs.microsoft.com/en-us/uwp/api/windows.ui.viewmanagement.uicolortype
  enum UIColorType {
    Accent = 0,     // The accent color.
    AccentDark1,    // The dark accent color.
    AccentDark2,    // The darker accent color.
    AccentDark3,    // The darkest accent color.
    AccentLight1,   // The light accent color.
    AccentLight2,   // The lighter accent color.
    AccentLight3,   // The lightest accent color.
    Background,     // The background color.
    Complement,     // Not supported. Do not use.
    Foreground,     // The foreground color.
  }

  // https://docs.microsoft.com/en-us/uwp/api/windows.ui.colors
  internal static class Colors {
    public static readonly Color AliceBlue = Color.FromArgb(0xFF, 0xF0, 0xF8, 0xFF);
    public static readonly Color AntiqueWhite = Color.FromArgb(0xFF, 0xFA, 0xEB, 0xD7);
    public static readonly Color Aqua = Color.FromArgb(0xFF, 0x00, 0xFF, 0xFF);
    public static readonly Color Aquamarine = Color.FromArgb(0xFF, 0x7F, 0xFF, 0xD4);
    public static readonly Color Azure = Color.FromArgb(0xFF, 0xF0, 0xFF, 0xFF);
    public static readonly Color Beige = Color.FromArgb(0xFF, 0xF5, 0xF5, 0xDC);
    public static readonly Color Bisque = Color.FromArgb(0xFF, 0xFF, 0xE4, 0xC4);
    public static readonly Color Black = Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
    public static readonly Color BlanchedAlmond = Color.FromArgb(0xFF, 0xFF, 0xEB, 0xCD);
    public static readonly Color Blue = Color.FromArgb(0xFF, 0x00, 0x00, 0xFF);
    public static readonly Color BlueViolet = Color.FromArgb(0xFF, 0x8A, 0x2B, 0xE2);
    public static readonly Color Brown = Color.FromArgb(0xFF, 0xA5, 0x2A, 0x2A);
    public static readonly Color BurlyWood = Color.FromArgb(0xFF, 0xDE, 0xB8, 0x87);
    public static readonly Color CadetBlue = Color.FromArgb(0xFF, 0x5F, 0x9E, 0xA0);
    public static readonly Color Chartreuse = Color.FromArgb(0xFF, 0x7F, 0xFF, 0x00);
    public static readonly Color Chocolate = Color.FromArgb(0xFF, 0xD2, 0x69, 0x1E);
    public static readonly Color Coral = Color.FromArgb(0xFF, 0xFF, 0x7F, 0x50);
    public static readonly Color CornflowerBlue = Color.FromArgb(0xFF, 0x64, 0x95, 0xED);
    public static readonly Color Cornsilk = Color.FromArgb(0xFF, 0xFF, 0xF8, 0xDC);
    public static readonly Color Crimson = Color.FromArgb(0xFF, 0xDC, 0x14, 0x3C);
    public static readonly Color Cyan = Color.FromArgb(0xFF, 0x00, 0xFF, 0xFF);
    public static readonly Color DarkBlue = Color.FromArgb(0xFF, 0x00, 0x00, 0x8B);
    public static readonly Color DarkCyan = Color.FromArgb(0xFF, 0x00, 0x8B, 0x8B);
    public static readonly Color DarkGoldenrod = Color.FromArgb(0xFF, 0xB8, 0x86, 0x0B);
    public static readonly Color DarkGray = Color.FromArgb(0xFF, 0xA9, 0xA9, 0xA9);
    public static readonly Color DarkGreen = Color.FromArgb(0xFF, 0x00, 0x64, 0x00);
    public static readonly Color DarkKhaki = Color.FromArgb(0xFF, 0xBD, 0xB7, 0x6B);
    public static readonly Color DarkMagenta = Color.FromArgb(0xFF, 0x8B, 0x00, 0x8B);
    public static readonly Color DarkOliveGreen = Color.FromArgb(0xFF, 0x55, 0x6B, 0x2F);
    public static readonly Color DarkOrange = Color.FromArgb(0xFF, 0xFF, 0x8C, 0x00);
    public static readonly Color DarkOrchid = Color.FromArgb(0xFF, 0x99, 0x32, 0xCC);
    public static readonly Color DarkRed = Color.FromArgb(0xFF, 0x8B, 0x00, 0x00);
    public static readonly Color DarkSalmon = Color.FromArgb(0xFF, 0xE9, 0x96, 0x7A);
    public static readonly Color DarkSeaGreen = Color.FromArgb(0xFF, 0x8F, 0xBC, 0x8F);
    public static readonly Color DarkSlateBlue = Color.FromArgb(0xFF, 0x48, 0x3D, 0x8B);
    public static readonly Color DarkSlateGray = Color.FromArgb(0xFF, 0x2F, 0x4F, 0x4F);
    public static readonly Color DarkTurquoise = Color.FromArgb(0xFF, 0x00, 0xCE, 0xD1);
    public static readonly Color DarkViolet = Color.FromArgb(0xFF, 0x94, 0x00, 0xD3);
    public static readonly Color DeepPink = Color.FromArgb(0xFF, 0xFF, 0x14, 0x93);
    public static readonly Color DeepSkyBlue = Color.FromArgb(0xFF, 0x00, 0xBF, 0xFF);
    public static readonly Color DimGray = Color.FromArgb(0xFF, 0x69, 0x69, 0x69);
    public static readonly Color DodgerBlue = Color.FromArgb(0xFF, 0x1E, 0x90, 0xFF);
    public static readonly Color Firebrick = Color.FromArgb(0xFF, 0xB2, 0x22, 0x22);
    public static readonly Color FloralWhite = Color.FromArgb(0xFF, 0xFF, 0xFA, 0xF0);
    public static readonly Color ForestGreen = Color.FromArgb(0xFF, 0x22, 0x8B, 0x22);
    public static readonly Color Fuchsia = Color.FromArgb(0xFF, 0xFF, 0x00, 0xFF);
    public static readonly Color Gainsboro = Color.FromArgb(0xFF, 0xDC, 0xDC, 0xDC);
    public static readonly Color GhostWhite = Color.FromArgb(0xFF, 0xF8, 0xF8, 0xFF);
    public static readonly Color Gold = Color.FromArgb(0xFF, 0xFF, 0xD7, 0x00);
    public static readonly Color Goldenrod = Color.FromArgb(0xFF, 0xDA, 0xA5, 0x20);
    public static readonly Color Gray = Color.FromArgb(0xFF, 0x80, 0x80, 0x80);
    public static readonly Color Green = Color.FromArgb(0xFF, 0x00, 0x80, 0x00);
    public static readonly Color GreenYellow = Color.FromArgb(0xFF, 0xAD, 0xFF, 0x2F);
    public static readonly Color Honeydew = Color.FromArgb(0xFF, 0xF0, 0xFF, 0xF0);
    public static readonly Color HotPink = Color.FromArgb(0xFF, 0xFF, 0x69, 0xB4);
    public static readonly Color IndianRed = Color.FromArgb(0xFF, 0xCD, 0x5C, 0x5C);
    public static readonly Color Indigo = Color.FromArgb(0xFF, 0x4B, 0x00, 0x82);
    public static readonly Color Ivory = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xF0);
    public static readonly Color Khaki = Color.FromArgb(0xFF, 0xF0, 0xE6, 0x8C);
    public static readonly Color Lavender = Color.FromArgb(0xFF, 0xE6, 0xE6, 0xFA);
    public static readonly Color LavenderBlush = Color.FromArgb(0xFF, 0xFF, 0xF0, 0xF5);
    public static readonly Color LawnGreen = Color.FromArgb(0xFF, 0x7C, 0xFC, 0x00);
    public static readonly Color LemonChiffon = Color.FromArgb(0xFF, 0xFF, 0xFA, 0xCD);
    public static readonly Color LightBlue = Color.FromArgb(0xFF, 0xAD, 0xD8, 0xE6);
    public static readonly Color LightCoral = Color.FromArgb(0xFF, 0xF0, 0x80, 0x80);
    public static readonly Color LightCyan = Color.FromArgb(0xFF, 0xE0, 0xFF, 0xFF);
    public static readonly Color LightGoldenrodYellow = Color.FromArgb(0xFF, 0xFA, 0xFA, 0xD2);
    public static readonly Color LightGray = Color.FromArgb(0xFF, 0xD3, 0xD3, 0xD3);
    public static readonly Color LightGreen = Color.FromArgb(0xFF, 0x90, 0xEE, 0x90);
    public static readonly Color LightPink = Color.FromArgb(0xFF, 0xFF, 0xB6, 0xC1);
    public static readonly Color LightSalmon = Color.FromArgb(0xFF, 0xFF, 0xA0, 0x7A);
    public static readonly Color LightSeaGreen = Color.FromArgb(0xFF, 0x20, 0xB2, 0xAA);
    public static readonly Color LightSkyBlue = Color.FromArgb(0xFF, 0x87, 0xCE, 0xFA);
    public static readonly Color LightSlateGray = Color.FromArgb(0xFF, 0x77, 0x88, 0x99);
    public static readonly Color LightSteelBlue = Color.FromArgb(0xFF, 0xB0, 0xC4, 0xDE);
    public static readonly Color LightYellow = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xE0);
    public static readonly Color Lime = Color.FromArgb(0xFF, 0x00, 0xFF, 0x00);
    public static readonly Color LimeGreen = Color.FromArgb(0xFF, 0x32, 0xCD, 0x32);
    public static readonly Color Linen = Color.FromArgb(0xFF, 0xFA, 0xF0, 0xE6);
    public static readonly Color Magenta = Color.FromArgb(0xFF, 0xFF, 0x00, 0xFF);
    public static readonly Color Maroon = Color.FromArgb(0xFF, 0x80, 0x00, 0x00);
    public static readonly Color MediumAquamarine = Color.FromArgb(0xFF, 0x66, 0xCD, 0xAA);
    public static readonly Color MediumBlue = Color.FromArgb(0xFF, 0x00, 0x00, 0xCD);
    public static readonly Color MediumOrchid = Color.FromArgb(0xFF, 0xBA, 0x55, 0xD3);
    public static readonly Color MediumPurple = Color.FromArgb(0xFF, 0x93, 0x70, 0xDB);
    public static readonly Color MediumSeaGreen = Color.FromArgb(0xFF, 0x3C, 0xB3, 0x71);
    public static readonly Color MediumSlateBlue = Color.FromArgb(0xFF, 0x7B, 0x68, 0xEE);
    public static readonly Color MediumSpringGreen = Color.FromArgb(0xFF, 0x00, 0xFA, 0x9A);
    public static readonly Color MediumTurquoise = Color.FromArgb(0xFF, 0x48, 0xD1, 0xCC);
    public static readonly Color MediumVioletRed = Color.FromArgb(0xFF, 0xC7, 0x15, 0x85);
    public static readonly Color MidnightBlue = Color.FromArgb(0xFF, 0x19, 0x19, 0x70);
    public static readonly Color MintCream = Color.FromArgb(0xFF, 0xF5, 0xFF, 0xFA);
    public static readonly Color MistyRose = Color.FromArgb(0xFF, 0xFF, 0xE4, 0xE1);
    public static readonly Color Moccasin = Color.FromArgb(0xFF, 0xFF, 0xE4, 0xB5);
    public static readonly Color NavajoWhite = Color.FromArgb(0xFF, 0xFF, 0xDE, 0xAD);
    public static readonly Color Navy = Color.FromArgb(0xFF, 0x00, 0x00, 0x80);
    public static readonly Color OldLace = Color.FromArgb(0xFF, 0xFD, 0xF5, 0xE6);
    public static readonly Color Olive = Color.FromArgb(0xFF, 0x80, 0x80, 0x00);
    public static readonly Color OliveDrab = Color.FromArgb(0xFF, 0x6B, 0x8E, 0x23);
    public static readonly Color Orange = Color.FromArgb(0xFF, 0xFF, 0xA5, 0x00);
    public static readonly Color OrangeRed = Color.FromArgb(0xFF, 0xFF, 0x45, 0x00);
    public static readonly Color Orchid = Color.FromArgb(0xFF, 0xDA, 0x70, 0xD6);
    public static readonly Color PaleGoldenrod = Color.FromArgb(0xFF, 0xEE, 0xE8, 0xAA);
    public static readonly Color PaleGreen = Color.FromArgb(0xFF, 0x98, 0xFB, 0x98);
    public static readonly Color PaleTurquoise = Color.FromArgb(0xFF, 0xAF, 0xEE, 0xEE);
    public static readonly Color PaleVioletRed = Color.FromArgb(0xFF, 0xDB, 0x70, 0x93);
    public static readonly Color PapayaWhip = Color.FromArgb(0xFF, 0xFF, 0xEF, 0xD5);
    public static readonly Color PeachPuff = Color.FromArgb(0xFF, 0xFF, 0xDA, 0xB9);
    public static readonly Color Peru = Color.FromArgb(0xFF, 0xCD, 0x85, 0x3F);
    public static readonly Color Pink = Color.FromArgb(0xFF, 0xFF, 0xC0, 0xCB);
    public static readonly Color Plum = Color.FromArgb(0xFF, 0xDD, 0xA0, 0xDD);
    public static readonly Color PowderBlue = Color.FromArgb(0xFF, 0xB0, 0xE0, 0xE6);
    public static readonly Color Purple = Color.FromArgb(0xFF, 0x80, 0x00, 0x80);
    public static readonly Color Red = Color.FromArgb(0xFF, 0xFF, 0x00, 0x00);
    public static readonly Color RosyBrown = Color.FromArgb(0xFF, 0xBC, 0x8F, 0x8F);
    public static readonly Color RoyalBlue = Color.FromArgb(0xFF, 0x41, 0x69, 0xE1);
    public static readonly Color SaddleBrown = Color.FromArgb(0xFF, 0x8B, 0x45, 0x13);
    public static readonly Color Salmon = Color.FromArgb(0xFF, 0xFA, 0x80, 0x72);
    public static readonly Color SandyBrown = Color.FromArgb(0xFF, 0xF4, 0xA4, 0x60);
    public static readonly Color SeaGreen = Color.FromArgb(0xFF, 0x2E, 0x8B, 0x57);
    public static readonly Color SeaShell = Color.FromArgb(0xFF, 0xFF, 0xF5, 0xEE);
    public static readonly Color Sienna = Color.FromArgb(0xFF, 0xA0, 0x52, 0x2D);
    public static readonly Color Silver = Color.FromArgb(0xFF, 0xC0, 0xC0, 0xC0);
    public static readonly Color SkyBlue = Color.FromArgb(0xFF, 0x87, 0xCE, 0xEB);
    public static readonly Color SlateBlue = Color.FromArgb(0xFF, 0x6A, 0x5A, 0xCD);
    public static readonly Color SlateGray = Color.FromArgb(0xFF, 0x70, 0x80, 0x90);
    public static readonly Color Snow = Color.FromArgb(0xFF, 0xFF, 0xFA, 0xFA);
    public static readonly Color SpringGreen = Color.FromArgb(0xFF, 0x00, 0xFF, 0x7F);
    public static readonly Color SteelBlue = Color.FromArgb(0xFF, 0x46, 0x82, 0xB4);
    public static readonly Color Tan = Color.FromArgb(0xFF, 0xD2, 0xB4, 0x8C);
    public static readonly Color Teal = Color.FromArgb(0xFF, 0x00, 0x80, 0x80);
    public static readonly Color Thistle = Color.FromArgb(0xFF, 0xD8, 0xBF, 0xD8);
    public static readonly Color Tomato = Color.FromArgb(0xFF, 0xFF, 0x63, 0x47);
    public static readonly Color Transparent = Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF);
    public static readonly Color Turquoise = Color.FromArgb(0xFF, 0x40, 0xE0, 0xD0);
    public static readonly Color Violet = Color.FromArgb(0xFF, 0xEE, 0x82, 0xEE);
    public static readonly Color Wheat = Color.FromArgb(0xFF, 0xF5, 0xDE, 0xB3);
    public static readonly Color White = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
    public static readonly Color WhiteSmoke = Color.FromArgb(0xFF, 0xF5, 0xF5, 0xF5);
    public static readonly Color Yellow = Color.FromArgb(0xFF, 0xFF, 0xFF, 0x00);
    public static readonly Color YellowGreen = Color.FromArgb(0xFF, 0x9A, 0xCD, 0x32);
  }

  internal class ColorsHelper : DispatcherObject {
    private const string AccentKey = "SystemAccentColor";
    private const string AccentDark1Key = "SystemAccentColorDark1";
    private const string AccentDark2Key = "SystemAccentColorDark2";
    private const string AccentDark3Key = "SystemAccentColorDark3";
    private const string AccentLight1Key = "SystemAccentColorLight1";
    private const string AccentLight2Key = "SystemAccentColorLight2";
    private const string AccentLight3Key = "SystemAccentColorLight3";

    internal static readonly Color DefaultAccentColor = Color.FromRgb(0x00, 0x78, 0xD7);

    private readonly ResourceDictionary _colors = new ResourceDictionary();

#if false // @TODO
    private UISettings _uiSettings;
#else
    private const bool light = true;
#endif

    private Color _systemBackground;
    private Color _systemAccent;

    private ColorsHelper() {
      if (SystemColorsSupported) {
        ListenToSystemColorChanges();
      }
    }

    public static bool SystemColorsSupported { get; } = OSVersionHelper.IsWindows10OrGreater;

    public static ColorsHelper Current { get; } = new ColorsHelper();

    public ResourceDictionary Colors => _colors;

    public ApplicationTheme? SystemTheme { get; private set; }

    public Color SystemAccentColor => _systemAccent;

    public event EventHandler SystemThemeChanged;

    public event EventHandler SystemAccentColorChanged;

    [MethodImpl(MethodImplOptions.NoInlining)]
    public void FetchSystemAccentColors() {
      #if false
      var uiSettings = new UISettings();
      _colors[AccentKey] = uiSettings.GetColorValue(UIColorType.Accent).ToColor();
      _colors[AccentDark1Key] = uiSettings.GetColorValue(UIColorType.AccentDark1).ToColor();
      _colors[AccentDark2Key] = uiSettings.GetColorValue(UIColorType.AccentDark2).ToColor();
      _colors[AccentDark3Key] = uiSettings.GetColorValue(UIColorType.AccentDark3).ToColor();
      _colors[AccentLight1Key] = uiSettings.GetColorValue(UIColorType.AccentLight1).ToColor();
      _colors[AccentLight2Key] = uiSettings.GetColorValue(UIColorType.AccentLight2).ToColor();
      _colors[AccentLight3Key] = uiSettings.GetColorValue(UIColorType.AccentLight3).ToColor();
      #else
      // Color from Windows 11 default theme
      _colors[AccentKey] = Color.FromArgb(0xFF, 0x00, 0x78, 0xD4);
      _colors[AccentDark1Key] = Color.FromArgb(0xFF, 0x00, 0x67, 0xC0);
      _colors[AccentDark2Key] = Color.FromArgb(0xFF, 0x00, 0x3E, 0x92);
      _colors[AccentDark3Key] = Color.FromArgb(0xFF, 0x00, 0x1A, 0x68);
      _colors[AccentLight1Key] = Color.FromArgb(0xFF, 0x00, 0x91, 0xF8);
      _colors[AccentLight2Key] = Color.FromArgb(0xFF, 0x4C, 0xC2, 0xFF);
      _colors[AccentLight3Key] = Color.FromArgb(0xFF, 0x99, 0xEB, 0xFF);
      #endif
    }

    public void SetAccent(Color accent) {
      Color color = accent;
      _colors[AccentKey] = color;
      UpdateShades(_colors, color);
    }

    public static void UpdateShades(ResourceDictionary colors, Color accent) {
      var palette = new ColorPalette(11, accent);
      colors[AccentDark1Key] = palette.Palette[6].ActiveColor;
      colors[AccentDark2Key] = palette.Palette[7].ActiveColor;
      colors[AccentDark3Key] = palette.Palette[8].ActiveColor;
      colors[AccentLight1Key] = palette.Palette[4].ActiveColor;
      colors[AccentLight2Key] = palette.Palette[3].ActiveColor;
      colors[AccentLight3Key] = palette.Palette[2].ActiveColor;
    }

    public static void RemoveShades(ResourceDictionary colors) {
      colors.Remove(AccentDark3Key);
      colors.Remove(AccentDark2Key);
      colors.Remove(AccentDark1Key);
      colors.Remove(AccentLight1Key);
      colors.Remove(AccentLight2Key);
      colors.Remove(AccentLight3Key);
    }

    public void UpdateBrushes(ResourceDictionary themeDictionary) {
      UpdateBrushes(themeDictionary, _colors);
    }

    public static void UpdateBrushes(ResourceDictionary themeDictionary, ResourceDictionary colors) {
      foreach (DictionaryEntry entry in themeDictionary) {
        if (entry.Value is SolidColorBrush brush && !brush.IsFrozen) {
          object colorKey = ThemeResourceHelper.GetColorKey(brush);
          if (colorKey != null && colors.Contains(colorKey)) {
            brush.SetCurrentValue(SolidColorBrush.ColorProperty, (Color)colors[colorKey]);
          }
        }
      }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void ListenToSystemColorChanges() {
#if false // @TODO
      _uiSettings = new UISettings();
      _uiSettings.ColorValuesChanged += OnColorValuesChanged;

      if (PackagedAppHelper.IsPackagedApp) {
        SystemEvents.UserPreferenceChanged += OnUserPreferenceChanged;
      }

      _systemBackground = _uiSettings.GetColorValue(UIColorType.Background).ToColor();
      _systemAccent = _uiSettings.GetColorValue(UIColorType.Accent).ToColor();
      UpdateSystemAppTheme();
#else
    _systemBackground =light ? Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF) :  Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
    _systemAccent = Color.FromArgb(0xFF, 0x00, 0x78, 0xD4);
    UpdateSystemAppTheme();
#endif
    }

#if false
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void OnColorValuesChanged(UISettings sender, object args) {
      Dispatcher.BeginInvoke(UpdateColorValues);
    }
#endif

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e) {
      if (e.Category == UserPreferenceCategory.General) {
        UpdateColorValues();
      }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void UpdateColorValues() {
      #if false // @TODO
      var background = _uiSettings.GetColorValue(UIColorType.Background).ToColor();
      if (_systemBackground != background) {
        _systemBackground = background;
        UpdateSystemAppTheme();
        SystemThemeChanged?.Invoke(null, EventArgs.Empty);
      }

      var accent = _uiSettings.GetColorValue(UIColorType.Accent).ToColor();
      if (_systemAccent != accent) {
        _systemAccent = accent;
        SystemAccentColorChanged?.Invoke(null, EventArgs.Empty);
      }
      #else

      var background = light ? Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF) :  Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
      if (_systemBackground != background) {
        _systemBackground = background;
        UpdateSystemAppTheme();
        SystemThemeChanged?.Invoke(null, EventArgs.Empty);
      }

      var accent = Color.FromArgb(0xFF, 0x00, 0x78, 0xD4);
      if (_systemAccent != accent) {
        _systemAccent = accent;
        SystemAccentColorChanged?.Invoke(null, EventArgs.Empty);
      }
      #endif
    }

    private void UpdateSystemAppTheme() {
      SystemTheme = IsDarkBackground(_systemBackground) ? ApplicationTheme.Dark : ApplicationTheme.Light;
    }

    private static bool IsDarkBackground(Color color) {
      return color.R + color.G + color.B < (255 * 3 - color.R - color.G - color.B);
    }
  }
}
