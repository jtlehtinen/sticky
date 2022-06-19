using System;
using System.Collections.Generic;
using System.Windows;

namespace Sticky {

  public class ThemeService {
    private const string THEME_NAME_KEY = "ThemeName";

    private Dictionary<string, ResourceDictionary> themes = new();

    public ThemeService() {
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

    public bool IsTheme(ResourceDictionary dic) {
      if (!dic.Contains(THEME_NAME_KEY)) return false;

      var name = dic[THEME_NAME_KEY] as string;

      return name != null && themes.ContainsKey(name);
    }
  }

}

