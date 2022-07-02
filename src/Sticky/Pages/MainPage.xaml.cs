using System.Windows.Controls;
using System.Windows.Input;
using ModernWpf.Controls;
using Sticky.ViewModels;

namespace Sticky {

  public partial class MainPage : UserControl {
    private DragBehavior _drag;

    public MainPage(MainPageViewModel vm) {
      DataContext = vm;

      InitializeComponent();
      _drag = new DragBehavior(this.TitleBar);
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

        //if (!string.IsNullOrEmpty(search)) {
        //  noteViewModel.Show = Search.ApplySearch(rtb, search);
        //} else {
        //  noteViewModel.Show = true;
        //}
      }
    }
  }

}
