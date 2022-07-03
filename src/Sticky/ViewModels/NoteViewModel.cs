using System;
using System.Windows.Input;
using Sticky.DataAccess;

namespace Sticky.ViewModels {

  public class NoteViewModel : ViewModelBase {
    public ICommand OpenCommand { get; }
    public ICommand CloseCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand OpenContextMenuCommand { get; }

    public event Action OpenContextMenuRequested;

    private Note _note;
    private Database _db;

    public NoteViewModel(Note note, Database db) {
      this._note = note;
      this._db = db;

      CloseCommand = new RelayCommand(() => IsOpen = false);
      OpenCommand = new RelayCommand(() => IsOpen = true);
      DeleteCommand = new RelayCommand(async () => {
        if (await Dialogs.ConfirmDelete()) {
          db.DeleteNote(_note);
        }
      });
      OpenContextMenuCommand = new RelayCommand(() => OpenContextMenuRequested?.Invoke());

      PropertyChanged += (sender, e) => _db.UpdateNote(_note);
    }

    public int Id { get { return _note.Id; } }

    public DateTime CreatedAt {
      get { return _note.CreatedAt; }
    }

    public bool Show { get; } = true;

    public string Content {
      get { return _note.Content; }
      set { _note.Content = value; OnPropertyChanged(); }
    }

    public string Theme {
      get { return _note.Theme; }
      set { _note.Theme = value; OnPropertyChanged(); }
    }

    public bool IsOpen {
      get { return _note.IsOpen; }
      set { _note.IsOpen = value; OnPropertyChanged(); }
    }

    public bool IsAlwaysOnTop {
      get { return _note.IsAlwaysOnTop; }
      set { _note.IsAlwaysOnTop = value; OnPropertyChanged(); }
    }

    public bool IsEmpty {
      get {
        // @TODO: ...
        const string EMPTY_NOTE = "\u003CSection xmlns=\u0022http://schemas.microsoft.com/winfx/2006/xaml/presentation\u0022 xml:space=\u0022preserve\u0022 TextAlignment=\u0022Left\u0022 LineHeight=\u0022Auto\u0022 IsHyphenationEnabled=\u0022False\u0022 xml:lang=\u0022en-us\u0022 FlowDirection=\u0022LeftToRight\u0022 NumberSubstitution.CultureSource=\u0022Text\u0022 NumberSubstitution.Substitution=\u0022AsCulture\u0022 FontFamily=\u0022Segoe UI\u0022 FontStyle=\u0022Normal\u0022 FontWeight=\u0022Normal\u0022 FontStretch=\u0022Normal\u0022 FontSize=\u002214\u0022 Foreground=\u0022#FF000000\u0022 Typography.StandardLigatures=\u0022True\u0022 Typography.ContextualLigatures=\u0022True\u0022 Typography.DiscretionaryLigatures=\u0022False\u0022 Typography.HistoricalLigatures=\u0022False\u0022 Typography.AnnotationAlternates=\u00220\u0022 Typography.ContextualAlternates=\u0022True\u0022 Typography.HistoricalForms=\u0022False\u0022 Typography.Kerning=\u0022True\u0022 Typography.CapitalSpacing=\u0022False\u0022 Typography.CaseSensitiveForms=\u0022False\u0022 Typography.StylisticSet1=\u0022False\u0022 Typography.StylisticSet2=\u0022False\u0022 Typography.StylisticSet3=\u0022False\u0022 Typography.StylisticSet4=\u0022False\u0022 Typography.StylisticSet5=\u0022False\u0022 Typography.StylisticSet6=\u0022False\u0022 Typography.StylisticSet7=\u0022False\u0022 Typography.StylisticSet8=\u0022False\u0022 Typography.StylisticSet9=\u0022False\u0022 Typography.StylisticSet10=\u0022False\u0022 Typography.StylisticSet11=\u0022False\u0022 Typography.StylisticSet12=\u0022False\u0022 Typography.StylisticSet13=\u0022False\u0022 Typography.StylisticSet14=\u0022False\u0022 Typography.StylisticSet15=\u0022False\u0022 Typography.StylisticSet16=\u0022False\u0022 Typography.StylisticSet17=\u0022False\u0022 Typography.StylisticSet18=\u0022False\u0022 Typography.StylisticSet19=\u0022False\u0022 Typography.StylisticSet20=\u0022False\u0022 Typography.Fraction=\u0022Normal\u0022 Typography.SlashedZero=\u0022False\u0022 Typography.MathematicalGreek=\u0022False\u0022 Typography.EastAsianExpertForms=\u0022False\u0022 Typography.Variants=\u0022Normal\u0022 Typography.Capitals=\u0022Normal\u0022 Typography.NumeralStyle=\u0022Normal\u0022 Typography.NumeralAlignment=\u0022Normal\u0022 Typography.EastAsianWidths=\u0022Normal\u0022 Typography.EastAsianLanguage=\u0022Normal\u0022 Typography.StandardSwashes=\u00220\u0022 Typography.ContextualSwashes=\u00220\u0022 Typography.StylisticAlternates=\u00220\u0022\u003E\u003CParagraph\u003E\u003CRun\u003E\u003C/Run\u003E\u003C/Paragraph\u003E\u003C/Section\u003E";
        return string.IsNullOrWhiteSpace(_note.Content) || _note.Content == EMPTY_NOTE;
      }
    }
  }

}
