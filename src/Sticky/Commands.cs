using System;
using System.Windows.Input;

namespace Sticky {

  public static class Commands {
    public static RoutedCommand OpenNoteList = new RoutedCommand("OpenNoteList", typeof(Commands));

    public static RoutedCommand About { get; } = new RoutedCommand("About", typeof(Commands));
    public static RoutedCommand NewNote { get; } = new RoutedCommand("NewNote", typeof(Commands));
    public static RoutedCommand DeleteNote { get; } = new RoutedCommand("DeleteNote", typeof(Commands));
    public static RoutedCommand CloseNote { get; } = new RoutedCommand("CloseNote", typeof(Commands));
    public static RoutedCommand OpenNote { get; } = new RoutedCommand("OpenNote", typeof(Commands));
    public static RoutedCommand ChangeAppTheme { get; } = new RoutedCommand("ChangeAppTheme", typeof(Commands));

    public static RoutedCommand ChangeNoteTheme { get; } = new RoutedCommand("ChangeNoteTheme", typeof(Commands));
    public static RoutedCommand ShowMenu { get; } = new RoutedCommand("ShowMenu", typeof(Commands));
    public static RoutedCommand ToggleTopmost { get; } = new RoutedCommand("ToggleTopmost", typeof(Commands));

    public static RoutedCommand OpenContextMenu { get; } = new RoutedCommand("OpenContextMenu", typeof(Commands));

    public static void Register(Type type, RoutedCommand command, ExecutedRoutedEventHandler executed) {
      CommandManager.RegisterClassCommandBinding(type, new CommandBinding(command, executed));
    }
  }

}
