using System.IO;
using System.Text.Json;

namespace Sticky {

  public static class Import {
    public static Model? FromJson(string filename) {
      if (!File.Exists(filename)) return new Model();

      using (var stream = new FileStream(filename, FileMode.Open)) {
        var model = JsonSerializer.Deserialize<Model>(stream);
        return model;
      }
    }
  }

  public static class Export {
    public static void ToJson(string filename, Model model) {
      var json = JsonSerializer.Serialize(model, new JsonSerializerOptions { WriteIndented = true });
      File.WriteAllText(filename, json);
    }
  }

}
