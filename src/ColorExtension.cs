using System.Windows.Media;

namespace Sticky {


  public static class ColorExtension {

    private static Color Blend(Color src, Color dest) {
      var A = src.A;
      var R = (src.R * A + dest.R * (255 - A)) / 255;
      var G = (src.G * A + dest.G * (255 - A)) / 255;
      var B = (src.B * A + dest.B * (255 - A)) / 255;
      return Color.FromArgb(255, (byte)R, (byte)G, (byte)B);
    }

    public static Color ToHoverColor(this Color background) {
      var c = Color.FromArgb(237, background.R, background.G, background.B);
      var dest = IsDarkColor(background) ? Colors.White : Colors.Black;
      return Blend(c, dest);
    }

    public static Color ToPressedColor(this Color background) {
      var c = Color.FromArgb(220, background.R, background.G, background.B);
      var dest = IsDarkColor(background) ? Colors.White : Colors.Black;
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
