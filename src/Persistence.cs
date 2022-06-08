using System.IO;
using System.Windows.Markup;

namespace Sticky {

  public static class Persistence {

    public static void Save(string filename, object obj) {
      using (var stream = new FileStream(filename, FileMode.OpenOrCreate)) {
        XamlWriter.Save(obj, stream);
      }
    }

    public static object? Load(string filename) {
      if (!File.Exists(filename)) return null;

      using (var stream = new FileStream(filename, FileMode.Open)) {
        return XamlReader.Load(stream);
      }
    }

  }

}
