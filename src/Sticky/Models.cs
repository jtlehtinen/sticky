using System;

namespace Sticky {

  public class Note {
    public int Id { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Theme { get; set; }
  }

}
