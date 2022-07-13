using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using ModernWpf;

namespace Sticky {

  public enum BaseTheme {
    Dark,
    Light,
    System
  }

  public class Themes {
    private const string THEME_NAME_KEY = "ThemeName";

    private Dictionary<string, ResourceDictionary> darkThemes = new();
    private Dictionary<string, ResourceDictionary> lightThemes = new();
    private Dictionary<string, ResourceDictionary> themes = new();

    private ResourceDictionary _globalDarkTheme = new();
    private ResourceDictionary _globalLightTheme = new();
    private ResourceDictionary _globalTheme = new();

    public event Action BaseThemeChanged;

    public Themes() {
      LoadThemes();
    }

    public Dictionary<string, ResourceDictionary> GetThemes() {
      return themes;
    }

    public bool UseDarkTheme() {
      var appTheme = ThemeManager.Current.ActualApplicationTheme;
      return appTheme == ApplicationTheme.Dark;
    }

    public bool UseLightTheme() {
      return !UseDarkTheme();
    }

    public ResourceDictionary GetGlobalTheme() {
      return _globalTheme;
    }

    private void UpdateThemes() {
      var sourceThemes = UseDarkTheme() ? darkThemes : lightThemes;

      foreach (var themeKey in sourceThemes.Keys) {
        if (!themes.ContainsKey(themeKey)) themes.Add(themeKey, new ResourceDictionary());

        var source = sourceThemes[themeKey];
        var dest = themes[themeKey];

        foreach (var key in source.Keys) {
          dest[key] = source[key];
        }
      }

      var globalSource = UseDarkTheme() ? _globalDarkTheme : _globalLightTheme;
      foreach (var key in globalSource.Keys) {
        _globalTheme[key] = globalSource[key];
      }
    }

    private void LoadThemes() {
      _globalDarkTheme = (ResourceDictionary)Application.LoadComponent(new Uri("Themes/GlobalDark.xaml", UriKind.Relative));
      _globalLightTheme = (ResourceDictionary)Application.LoadComponent(new Uri("Themes/GlobalLight.xaml", UriKind.Relative));

      // @IMPORTANT: The filename order is set on purpose.
      string[] filenames = new[]{"Yellow.xaml", "Green.xaml", "Pink.xaml", "Purple.xaml", "Blue.xaml", "Gray.xaml", "Charcoal.xaml"};

      var suffixLength = ".xaml".Length;

      foreach (var theme in filenames) {
        var path = "Themes/" + theme;
        var dic = (ResourceDictionary)Application.LoadComponent(new Uri(path, UriKind.Relative));
        if (dic == null) continue;

        var dark = dic["Dark"] as ResourceDictionary;
        var light = dic["Light"] as ResourceDictionary;

        var name = theme.Substring(0, theme.Length - suffixLength);
        dark.Add(THEME_NAME_KEY, name);
        light.Add(THEME_NAME_KEY, name);

        darkThemes[name] = dark;
        lightThemes[name] = light;
      }

      UpdateThemes();
    }

    public ResourceDictionary? GetTheme(string themeName) {
      if (themeName == null) return null;

      return themes.GetValueOrDefault(themeName);
    }

    public string GetThemeName(ResourceDictionary dic) {
      var name = (string)dic[THEME_NAME_KEY];
      return name;
    }

    public bool IsValidThemeName(string name) {
      return themes.ContainsKey(name);
    }

    public bool IsTheme(ResourceDictionary dic) {
      if (!dic.Contains(THEME_NAME_KEY)) return false;

      var name = dic[THEME_NAME_KEY] as string;

      return name != null && themes.ContainsKey(name);
    }

    public void SetBaseTheme(BaseTheme theme) {
      var appTheme = theme.ConvertToApplicationTheme();
      if (appTheme != ThemeManager.Current.ActualApplicationTheme) {
        ThemeManager.Current.ApplicationTheme = appTheme;
        UpdateThemes();
        BaseThemeChanged?.Invoke();
      }
    }
  }

}

