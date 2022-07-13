using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;


namespace Sticky {

  public static class Search {
    public static bool Highlight(RichTextBox rtb, string search, SolidColorBrush highlightBrush) {
      var matchFound = false;

      for (var start = rtb.Document.ContentStart; start.CompareTo(rtb.Document.ContentEnd) <= 0; start = start.GetNextContextPosition(LogicalDirection.Forward)) {
        if (start.CompareTo(rtb.Document.ContentEnd) == 0) {
          break;
        }

        var text = start.GetTextInRun(LogicalDirection.Forward);

        int index = text.IndexOf(search, StringComparison.InvariantCultureIgnoreCase);
        if (index >= 0) {
          start = start.GetPositionAtOffset(index);

          if (start != null) {
            var end = start.GetPositionAtOffset(search.Length);
            var range = new TextRange(start, end);

            range.ApplyPropertyValue(TextElement.BackgroundProperty, highlightBrush);
            matchFound = true;
          }
        }
      }

      return matchFound;
    }

    public static void ClearHighlight(RichTextBox rtb) {
      var range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
      range.ApplyPropertyValue(TextElement.BackgroundProperty, null);
    }

    // https://stackoverflow.com/questions/636383/how-can-i-find-wpf-controls-by-name-or-type
    public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject {
      // Confirm parent and childName are valid.
      if (parent == null) return null;

      T foundChild = null;

      int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
      for (int i = 0; i < childrenCount; i++) {
        var child = VisualTreeHelper.GetChild(parent, i);
        // If the child is not of the request child type child
        T childType = child as T;
        if (childType == null) {
          // Recursively drill down the tree
          foundChild = FindChild<T>(child, childName);

          // If the child is found, break so we do not overwrite the found child.
          if (foundChild != null) break;
        } else if (!string.IsNullOrEmpty(childName)) {
          var frameworkElement = child as FrameworkElement;
          // If the child's name is set for search
          if (frameworkElement != null && frameworkElement.Name == childName) {
            // If the child's name is of the request name
            foundChild = (T)child;
            break;
          }
        } else {
          // Child element found.
          foundChild = (T)child;
          break;
        }
      }

      return foundChild;
    }
  }

}
