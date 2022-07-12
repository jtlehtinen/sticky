
namespace Sticky.DataAccess {

  public static class NoteFactory {

    public static Note CreateNote(Settings settings) {
      var result = new Note {
        IsAlwaysOnTop = settings.PinNewNote,
      };

      return result;
    }

  }

}
