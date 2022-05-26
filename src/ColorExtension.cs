using System.Windows.Media;

namespace Sticky {

  public static class ColorExtension {

    public static Color ToHoverColor(this Color background) {
      return Color.FromArgb(255, 255, 0, 0);
    }

    public static Color ToPressedColor(this Color background) {
      return Color.FromArgb(255, 0, 255, 0);
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
