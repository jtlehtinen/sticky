using System;
using System.Linq;
using System.Collections.ObjectModel;

namespace Sticky {

  public static class ObservableCollectionExtensions {

    public static int RemoveAll<T>(this ObservableCollection<T> collection, Func<T, bool> predicate) {
      var remove = collection.Where(predicate).ToList();

      foreach (var item in remove) {
        collection.Remove(item);
      }

      return remove.Count;
    }

  }

}
