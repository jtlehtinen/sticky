using System.Threading.Tasks;
using ModernWpf.Controls;

namespace Sticky {

  public record ConfirmDeleteResult(bool DoDelete, bool DontAskAgain) { }

  public static class Dialogs {

    public static async Task<ConfirmDeleteResult> ConfirmDelete() {
      var dialog = new ConfirmDeleteDialog();
      var result = await dialog.ShowAsync();

      var doDelete = (result == ContentDialogResult.Primary);
      var dontAskAgain = dialog.ShouldNotAskAgain();

      return new ConfirmDeleteResult(doDelete, dontAskAgain);
    }

  }

}
