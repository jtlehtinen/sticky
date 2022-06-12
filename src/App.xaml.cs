using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace Sticky {

  public class State {
    public List<Note> Notes { get; set; } = new List<Note>();

    public State() {
      var notes = Notes;
      notes.Add(new Note(){
        Id = 1,
        CreatedAt = new DateTime(2022, 6, 8, 14, 30, 0),
        Content = "<FlowDocument xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" PagePadding=\"5,0,5,0\" AllowDrop=\"True\"><Paragraph><Bold>Note 1</Bold></Paragraph><Paragraph>This is note 1.</Paragraph></FlowDocument>"
      });
      notes.Add(new Note(){
        Id = 2,
        CreatedAt = new DateTime(2022, 6, 7, 15, 30, 0),
        Content = "<FlowDocument xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" PagePadding=\"5,0,5,0\" AllowDrop=\"True\"><Paragraph><Bold>Note 2</Bold></Paragraph><Paragraph>This is note 2.</Paragraph></FlowDocument>"
      });
    }
  }

  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application {
    // @TODO: Better way to detect if a resource dictionary
    // is a theme.
    const string THEME_KEY = "THEME-MARKER-KEY";
    const string THEME_VALUE = "THEME-MARKER-VALUE";

    public State state;
    private Mutex singleInstanceMutex;

    private void Main(object sender, StartupEventArgs args) {
      var createdNewMutex = false;
      singleInstanceMutex = new Mutex(true, "cbe6e195-20b6-4950-97fa-6da85c3715f8", out createdNewMutex);
      if (!createdNewMutex) {
        Shutdown();
        return;
      }

      state = new State();

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

      var theme = (ResourceDictionary?)Properties[themeName];
      Resources.MergedDictionaries.Add(theme);
    }
  }
}
