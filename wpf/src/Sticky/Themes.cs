using System;
using System.Collections.Generic;
using System.Windows;
using ModernWpf;

namespace Sticky {

  public enum BaseTheme {
    Dark,
    Light,
    System
  }

  public class Themes {
    private const string THEME_NAME_KEY = "ThemeName";

    private Dictionary<string, ResourceDictionary> themes = new();

    public Themes() {
      LoadThemes();
    }

    private void LoadThemes() {
      string[] filenames = new[]{"Blue.xaml", "Charcoal.xaml", "Gray.xaml", "Green.xaml", "Pink.xaml", "Purple.xaml", "Yellow.xaml"};

      foreach (var theme in filenames) {
        var path = "Themes/" + theme;
        var dic = (ResourceDictionary)Application.LoadComponent(new Uri(path, UriKind.Relative));
        if (dic == null) continue;

        var name = dic.Contains(THEME_NAME_KEY) ? dic[THEME_NAME_KEY] as string : null;
        if (name == null) continue;

        themes[name] = dic;
      }
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
      var appTheme = theme.ToApplicationTheme();
      if (appTheme != ThemeManager.Current.ActualApplicationTheme) {
        ThemeManager.Current.ApplicationTheme = appTheme;
      }
    }
  }

}

