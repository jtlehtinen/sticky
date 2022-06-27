using System.Windows.Controls;
using ModernWpf.Controls;

namespace Sticky {

  public partial class MainPage : UserControl {
    public MainPage() {
      InitializeComponent();
    }

    private void OnSearchQueryChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs e) {
      if (e.Reason != AutoSuggestionBoxTextChangeReason.UserInput) return;

      DoSearch(sender.Text);
    }

    private void OnSearchQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs e) {
      DoSearch(e.QueryText);
    }

    private void DoSearch(string search) {
      var count = NoteList.Items.Count;
      for (var i = 0; i < count; ++i) {
        var container = NoteList.ItemContainerGenerator.ContainerFromIndex(i) as ContentPresenter;
        if (container == null) continue;

        var noteViewModel = NoteList.ItemContainerGenerator.ItemFromContainer(container) as NoteViewModel;
        if (noteViewModel == null) continue;

        var rtb = Search.FindChild<RichTextBox>(container, "NoteRichTextBox");
        if (rtb == null) continue;

        Search.ClearSearch(rtb);

        if (!string.IsNullOrEmpty(search)) {
          noteViewModel.Show = Search.ApplySearch(rtb, search);
        } else {
          noteViewModel.Show = true;
        }
      }
    }
  }

}
