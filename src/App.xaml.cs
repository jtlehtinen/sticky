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
    public const string THEME_NAME_KEY = "ThemeName";
    public const string THEME_MARKER_KEY = "THEME-MARKER-KEY";
    public const string THEME_MARKER_VALUE = "THEME-MARKER-VALUE";

    public State? state;
    private Mutex? singleInstanceMutex;

    private void Main(object sender, StartupEventArgs args) {
      var createdNewMutex = false;
      singleInstanceMutex = new Mutex(true, "cbe6e195-20b6-4950-97fa-6da85c3715f8", out createdNewMutex);
      if (!createdNewMutex) {
        Shutdown();
        return;
      }

      state = new State();

      LoadThemes();

      var window = new MainWindow();
      window.Show();
    }

    private void LoadThemes() {
      var themes = new string[]{"Blue.xaml", "Charcoal.xaml", "Gray.xaml", "Green.xaml", "Pink.xaml", "Purple.xaml", "Yellow.xaml"};

      foreach (var theme in themes) {
        var path = "Themes/" + theme;
        var dic = (ResourceDictionary)LoadComponent(new Uri(path, UriKind.Relative));
        if (dic == null) continue;

        var name = dic.Contains(THEME_NAME_KEY) ? dic[THEME_NAME_KEY] as string : null;
        if (name == null) continue;

        Properties[name] = dic;
      }
    }
  }
}
