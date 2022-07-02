using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ModernWpf;

namespace Sticky {

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum BaseTheme {
    Dark,
    Light,
    System
  }

  public static class BaseThemeExtensions {
    public static ApplicationTheme? ToApplicationTheme(this BaseTheme theme) {
      if (theme == BaseTheme.Dark) return ApplicationTheme.Dark;
      if (theme == BaseTheme.Light) return ApplicationTheme.Light;
      return null;
    }
  }

}
