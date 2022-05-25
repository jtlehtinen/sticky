using System;
using System.Windows;
using System.Windows.Input;

namespace Sticky {
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application {
    const string THEME_KEY = "THEME-MARKER-KEY";
    const string THEME_VALUE = "THEME-MARKER-VALUE";

    private void Main(object sender, StartupEventArgs args) {
      this.Properties["Themes.Blue"] = LoadTheme("Themes/Blue.xaml");
      this.Properties["Themes.Charcoal"] = LoadTheme("Themes/Charcoal.xaml");
      this.Properties["Themes.Gray"] = LoadTheme("Themes/Gray.xaml");
      this.Properties["Themes.Green"] = LoadTheme("Themes/Green.xaml");
      this.Properties["Themes.Pink"] = LoadTheme("Themes/Pink.xaml");
      this.Properties["Themes.Purple"] = LoadTheme("Themes/Purple.xaml");
      this.Properties["Themes.Yellow"] = LoadTheme("Themes/Yellow.xaml");

      var window = new MainWindow();
      window.Show();
    }

    private ResourceDictionary? LoadTheme(string path) {
      var dic = (ResourceDictionary)LoadComponent(new Uri(path, UriKind.Relative));
      if (dic != null) dic.Add(THEME_KEY, THEME_VALUE);

      return dic;
    }

    public void SetTheme(string themeName) {
      if (!Properties.Contains(themeName)) return;

      for (var i = Resources.MergedDictionaries.Count - 1; i >= 0; --i) {
        var dic = Resources.MergedDictionaries[i];
        var isTheme = dic.Contains(THEME_KEY);
        if (isTheme) {
          Resources.MergedDictionaries.Remove(dic);
        } else {
          dic.Source = dic.Source;
        }
      }

      var theme = (ResourceDictionary)Properties[themeName];
      Resources.MergedDictionaries.Add(theme);
    }
  }
}
