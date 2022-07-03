using System.Threading.Tasks;
using ModernWpf.Controls;

namespace Sticky {

  public static class Dialogs {

    public static async Task<bool> ConfirmDelete() {
      var dialog = new ConfirmDeleteDialog();
      var result = await dialog.ShowAsync();

      var doDelete = (result == ContentDialogResult.Primary);
      return doDelete;
    }

  }

}
