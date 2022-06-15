using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace Sticky {

  public static class Import {
    public static List<Note>? FromJson(string filename) {
      using (var stream = new FileStream(filename, FileMode.Open)) {
        var notes = JsonSerializer.Deserialize<List<Note>>(stream);
        return notes;
      }
    }
  }

  public static class Export {
    public static void ToJson(string filename, List<Note> notes) {
      var json = JsonSerializer.Serialize(notes, new JsonSerializerOptions { WriteIndented = true });
      File.WriteAllText(filename, json);
    }
  }

}
