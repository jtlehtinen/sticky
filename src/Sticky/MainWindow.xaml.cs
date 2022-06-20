using System.Linq;
using System.Windows;
using System.Collections.Generic;
using ModernWpf.Controls;

namespace Sticky {

  public class SearchItem {
    public string Name { get; set; } = "";

    public override string ToString() {
      return Name;
    }
  }

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {

    public MainWindow() {
      DataContext = App.Current.ViewModel;

      InitializeComponent();
      Native.ApplyRoundedWindowCorners(this);
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e) {
      // @NOTE: Hack! When the window is maximized the window size ends
      // up being greater than the monitor size.
      var thickness = (this.WindowState == WindowState.Maximized ? 8 : 0);
      this.BorderThickness = new System.Windows.Thickness(thickness);
    }

    private void OnSearchTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs e) {
      if (e.Reason != AutoSuggestionBoxTextChangeReason.UserInput) return;

      System.Console.WriteLine("OnSearchTextChanged");

      var suggestions = DoSearch(sender.Text);
      sender.ItemsSource = suggestions.Count > 0 ? suggestions : new string[] { "No result found" };
    }

    private void OnSearchQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs e) {
      System.Console.WriteLine("OnSearchQuerySubmitted");

      if (e.ChosenSuggestion != null && e.ChosenSuggestion is SearchItem) {
        DoSelect(e.ChosenSuggestion as SearchItem);
      } else if (!string.IsNullOrEmpty(e.QueryText)) {
        var suggestions = DoSearch(sender.Text);
        if (suggestions.Count > 0) {
          DoSelect(suggestions.FirstOrDefault());
        }
      }
    }

    private void OnSearchSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs e) {
      System.Console.WriteLine("OnSearchSuggestionChosen");
    }

    private void OnAutoSuggestBoxItemLinkClicked(object sender, RoutedEventArgs e) {
      System.Console.WriteLine("ControlLink_Click");
    }

    private void DoSelect(SearchItem? item) {
      if (item == null) return;

      AutoSuggestBoxItemDropDown.Visibility = Visibility.Visible;
      AutoSuggestBoxItemTitle.Text = item.Name;
      AutoSuggestBoxItemLink.Content = "Go to " + item.Name;
      AutoSuggestBoxItemLink.Tag = item.Name;
    }

    private List<SearchItem> DoSearch(string query) {
      if (string.IsNullOrEmpty(query)) return new();
      if (!query.StartsWith("foo")) return new();
      return new(){
        new SearchItem{Name = "foo"},
        new SearchItem{Name = "foobar"},
      };
    }
  }
}
