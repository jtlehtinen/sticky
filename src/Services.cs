using System;
using System.Collections.Generic;
using System.Windows;

namespace Sticky {

  public interface IServiceProvider {
    T? GetService<T>();
  }

  public sealed class ServiceCollection : IServiceProvider {
    private Dictionary<Type, Object> services = new();

    public T? GetService<T>() {
      if (!services.ContainsKey(typeof(T))) return default(T);

      return (T)services[typeof(T)];
    }

    public void AddService<T>(T service) {
      if (service == null) {
        throw new ArgumentNullException("service");
      }

      if (services.ContainsKey(typeof(T))) {
        throw new InvalidOperationException($"Service of type {typeof(T)} already exists.");
      }

      services[typeof(T)] = service;
    }
  }

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

