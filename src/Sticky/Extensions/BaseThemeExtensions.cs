using ModernWpf;

namespace Sticky {

  public static class BaseThemeExtensions {
    public static ApplicationTheme? ConvertToApplicationTheme(this BaseTheme theme) {
      if (theme == BaseTheme.Dark) return ApplicationTheme.Dark;
      if (theme == BaseTheme.Light) return ApplicationTheme.Light;
      return null;
    }
  }

}

