using System;
using System.Text.Json.Serialization;

namespace Sticky {

  public class Note {
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("theme")]
    public string? Theme { get; set; }
  }

}
