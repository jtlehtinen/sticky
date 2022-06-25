using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sticky {

  public class Note {
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; } = "";

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("theme")]
    public string Theme { get; set; } = "";

    public Note() {
      Content = "";
      CreatedAt = DateTime.Now;
      Theme = "Theme.Pink";
    }
  }

  public class Model {
    public List<Note> Notes { get; set; } = new();

    public Model() {
      Load();
    }

    public Note CreateNote() {
      var note = new Note { Id = NextNoteId() };
      Notes.Add(note);
      return note;
    }

    private int NextNoteId() {
      var max = Notes.MaxBy(n => n.Id);
      return max?.Id + 1 ?? 1;
    }

    private void Load() {
      Notes.Clear();

      var list = Import.FromJson("sticky.json") ?? new List<Note>();
      foreach (var note in list) {
        Notes.Add(note);
      }
    }

    public void Save() {
      Export.ToJson("sticky.json", Notes);
    }
  }

}
