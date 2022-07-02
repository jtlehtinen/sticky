using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Sticky {

  public static class ObservableCollectionExtensions {
    public static int RemoveAll<T>(this ObservableCollection<T> collection, Func<T, bool> predicate) {
      var remove = collection.Where(predicate).ToList();

      foreach (var item in collection.Where(predicate).ToList()) {
        collection.Remove(item);
      }

      return remove.Count;
    }
  }

  public static class ColorExtension {

    private static Color Blend(Color src, Color dest) {
      var A = src.A;
      var R = (src.R * A + dest.R * (255 - A)) / 255;
      var G = (src.G * A + dest.G * (255 - A)) / 255;
      var B = (src.B * A + dest.B * (255 - A)) / 255;
      return Color.FromArgb(255, (byte)R, (byte)G, (byte)B);
    }

    public static Color ToInactiveColor(this Color color) {
      // @TODO: Make better?
      return Color.FromArgb(0x66, color.R, color.G, color.B);
    }

    public static Color ToHoverColor(this Color color) {
      var c = Color.FromArgb(237, color.R, color.G, color.B);
      var dest = IsDarkColor(color) ? Colors.White : Colors.Black;
      return Blend(c, dest);
    }

    public static Color ToPressedColor(this Color color) {
      var c = Color.FromArgb(220, color.R, color.G, color.B);
      var dest = IsDarkColor(color) ? Colors.White : Colors.Black;
      return Blend(c, dest);
    }

    public static bool IsDarkColor(Color c) {
      // @TODO: Convert to linear space first?
      return (5 * c.G + 2 * c.R + c.B) <= 8 * 128;
    }

    public static bool IsLightColor(Color c) {
      return !IsDarkColor(c);
    }

  }

}
