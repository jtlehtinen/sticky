using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sticky {

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum BaseTheme { Dark, Light, System }

  public class Note {
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; } = "";

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [JsonPropertyName("theme")]
    public String Theme { get; set; } = "Theme.Yellow";

    [JsonPropertyName("pinned")]
    public bool Pinned { get; set; } = false;
  }

  public class Settings {
    [JsonPropertyName("pin_new_note")]
    public bool PinNewNote { get; set; } = false;

    [JsonPropertyName("confirm_before_delete")]
    public bool ConfirmBeforeDelete { get; set; } = true;

    [JsonPropertyName("base_theme")]
    public BaseTheme BaseTheme { get; set; } = BaseTheme.Light;
  }

  public class Model {
    [JsonPropertyName("notes")]
    public List<Note> Notes { get; set; } = new();

    [JsonPropertyName("settings")]
    public Settings Settings { get; set; } = new();

    public Note CreateNote() {
      var note = new Note { Id = NextNoteId() };
      Notes.Add(note);
      return note;
    }

    private int NextNoteId() {
      var max = Notes.MaxBy(n => n.Id);
      return max?.Id + 1 ?? 1;
    }
  }

}
