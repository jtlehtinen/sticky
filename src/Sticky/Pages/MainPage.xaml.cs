using System.Windows.Controls;
using System.Windows.Media;
using ModernWpf.Controls;
using Sticky.ViewModels;

namespace Sticky {

  public partial class MainPage : UserControl {
    private SolidColorBrush _highlightBrush = new SolidColorBrush(Colors.Yellow);
    private DragBehavior _drag;

    public MainPage(MainPageViewModel viewModel) {
      DataContext = viewModel;

      Resources.MergedDictionaries.Add(App.Current.Themes.GetGlobalTheme());

      InitializeComponent();
      _drag = new DragBehavior(PartTitleBar);
    }

    private void OnSearchQueryChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs e) {
      if (e.Reason != AutoSuggestionBoxTextChangeReason.UserInput) return;

      DoSearch(sender.Text);
    }

    private void OnSearchQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs e) {
      DoSearch(e.QueryText);
    }

    private void DoSearch(string search) {
      #if false
      var count = NoteList.Items.Count;
      for (var i = 0; i < count; ++i) {
        var container = NoteList.ItemContainerGenerator.ContainerFromIndex(i) as ContentPresenter;
        if (container == null) continue;

        var noteViewModel = NoteList.ItemContainerGenerator.ItemFromContainer(container) as NoteViewModel;
        if (noteViewModel == null) continue;

        var rtb = Search.FindChild<RichTextBox>(container, "NoteRichTextBox");
        if (rtb == null) continue;

        Search.ClearHighlight(rtb);
        var wasHighlighted = Search.Highlight(rtb, search, _highlightBrush);
        if (!wasHighlighted) {
          // @TODO: filter out the note...
        }
      }
      #endif
    }
  }

}
