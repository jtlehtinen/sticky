using System;
using System.Windows;

namespace Sticky.Extensions {

  public static class WindowCollectionExtensions {

    /// <summary>
    /// Find method returns the first element in the provided window collection
    /// that satisfies the provided predicate. If no window satisifes the predicate,
    /// null is returned.
    /// </summary>
    public static Window? Find(this WindowCollection collection, Func<Window, bool> predicate) {
      foreach (Window window in collection) {
        if (predicate(window)) {
          return window;
        }
      }
      return null;
    }

  }

}
