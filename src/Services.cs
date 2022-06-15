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

  public class NoteService {
    private Dictionary<int, Note> notes = new();

    public NoteService() {
      AddPlaceholderNotes();
    }

    private void AddPlaceholderNotes() {
      var note = new Note {
        Id = 1,
        CreatedAt = new DateTime(2022, 6, 8, 14, 30, 0),
        Content = "<FlowDocument xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" PagePadding=\"5,0,5,0\" AllowDrop=\"True\"><Paragraph><Bold>Note 1</Bold></Paragraph><Paragraph>This is note 1.</Paragraph></FlowDocument>"
      };
      notes.Add(note.Id, note);

      note = new Note {
        Id = 2,
        CreatedAt = new DateTime(2022, 6, 7, 15, 30, 0),
        Content = "<FlowDocument xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" PagePadding=\"5,0,5,0\" AllowDrop=\"True\"><Paragraph><Bold>Note 2</Bold></Paragraph><Paragraph>This is note 2.</Paragraph></FlowDocument>"
      };
      notes.Add(note.Id, note);
    }

    public Note? GetNote(int id) {
      return notes.GetValueOrDefault(id);
    }

    public List<Note> GetNotes() {
      return new List<Note>(notes.Values);
    }
  }

}

