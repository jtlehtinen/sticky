using System;
using System.IO;
using System.Collections.Generic;
using SQLite;

namespace Sticky.DataAccess {

  // @TODO: Caching maybe...

  [Table("Notes")]
  public class Note {
    [PrimaryKey, AutoIncrement]
    [Column("Id")]
    public int Id { get; set; }

    [Column("Content")]
    public string Content { get; set; } = "";

    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; }

    [Column("UpdatedAt")]
    public DateTime UpdatedAt { get; set; }

    [Column("Theme")]
    public string Theme { get; set; } = "Yellow";

    [Column("IsAlwaysOnTop")]
    public bool IsAlwaysOnTop { get; set; }

    [Column("IsOpen")]
    public bool IsOpen { get; set; } = true;

    [Column("WindowPosition")]
    public string WindowPosition { get; set; }

    public Note() {
      var now = DateTime.Now;
      CreatedAt = now;
      UpdatedAt = now;
    }

    public bool IsEmpty() {
      if (string.IsNullOrWhiteSpace(Content) || Content == RichTextBoxHelper.EMPTY_FLOW_DOCUMENT_XAML) return true;

      // @TODO: ugh...
      var doc = RichTextBoxHelper.DeserializeFromXaml(Content);
      return RichTextBoxHelper.IsFlowDocumentEmpty(doc);
    }
  }

  [Table("Settings")]
  public class Settings {
    [PrimaryKey, AutoIncrement]
    [Column("Id")]
    public int Id { get; set; }

    [Column("PinNewNote")]
    public bool PinNewNote { get; set; } = false;

    [Column("ConfirmBeforeDelete")]
    public bool ConfirmBeforeDelete { get; set; } = true;

    [Column("BaseTheme")]
    public BaseTheme BaseTheme { get; set; } = BaseTheme.Light;

    [Column("WindowPosition")]
    public string MainWindowPosition { get; set; }
  }

  public class NoteAddedEventArgs : EventArgs {
    public NoteAddedEventArgs(Note addedNote) { this.AddedNote = addedNote; }

    public Note AddedNote { get; }
  }

  public class NoteDeletedEventArgs : EventArgs {
    public NoteDeletedEventArgs(Note deletedNote) { this.DeletedNote = deletedNote; }

    public Note DeletedNote { get; }
  }

  public class NoteModifiedEventArgs : EventArgs {
    public NoteModifiedEventArgs(Note modifiedNote) { this.ModifiedNote = modifiedNote; }

    public Note ModifiedNote { get; }
  }

  public class SettingsModifiedEventArgs : EventArgs {
    public SettingsModifiedEventArgs(Settings newSettings) { this.NewSettings = newSettings; }

    public Settings NewSettings { get; }
  }

  public class Database {
    private SQLiteConnection _db;

    public Database() {
      var directory = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "StickyNotes");
      var fullpath = Path.Combine(directory, "sticky.db");

      Directory.CreateDirectory(directory);

      _db = new SQLiteConnection(fullpath);
      _db.CreateTable<Note>();
      _db.CreateTable<Settings>();

      EnsureSettingsExists();
    }

    public event EventHandler<NoteAddedEventArgs> NoteAdded;
    public event EventHandler<NoteDeletedEventArgs> NoteDeleted;
    public event EventHandler<NoteModifiedEventArgs> NoteModified;

    public event EventHandler<SettingsModifiedEventArgs> SettingsModified;

    public Note GetNote(int id) => _db.Get<Note>(id);
    public List<Note> GetNotes() => _db.Query<Note>("SELECT * FROM Notes");

    public void AddNote(Note note) {
      if (note == null)
        throw new ArgumentNullException("note");

      _db.Insert(note);
      NoteAdded?.Invoke(this, new NoteAddedEventArgs(note));
    }

    public void DeleteNote(Note note) {
      _db.Delete<Note>(note.Id);
      NoteDeleted?.Invoke(this, new NoteDeletedEventArgs(note));
    }

    public void UpdateNote(Note note, bool notify = true) {
      if (!note.IsEmpty()) {
        var prevNote = GetNote(note.Id);
        if (prevNote == null || note.Content != prevNote.Content) {
          note.UpdatedAt = DateTime.Now;
        }
      }

      _db.Update(note);
      if (notify) NoteModified?.Invoke(this, new NoteModifiedEventArgs(note));
    }

    public Settings GetSettings() {
      // @NOTE: Always call EnsureSettingsExists() before calling
      // this method.
      return _db.Query<Settings>("SELECT * FROM Settings")[0];
    }

    public void UpdateSettings(Settings settings) {
      _db.Update(settings);
      SettingsModified?.Invoke(this, new SettingsModifiedEventArgs(settings));
    }

    private void EnsureSettingsExists() {
      var settings = _db.Query<Settings>("SELECT * FROM Settings");
      if (settings.Count == 0) {
        _db.Insert(new Settings());
      }
    }
  }

}
