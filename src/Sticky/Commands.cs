using System;
using System.Windows.Input;

namespace Sticky {

  public static class Commands {
    public static RoutedCommand About { get; } = new RoutedCommand("About", typeof(Commands));
    public static RoutedCommand NewNote { get; } = new RoutedCommand("NewNote", typeof(Commands));
    public static RoutedCommand ToggleAppTheme { get; } = new RoutedCommand("ToggleAppTheme", typeof(Commands));

    public static RoutedCommand ChangeNoteTheme { get; } = new RoutedCommand("ChangeNoteTheme", typeof(Commands));
    public static RoutedCommand ShowMenu { get; } = new RoutedCommand("ShowMenu", typeof(Commands));
    public static RoutedCommand ToggleTopmost { get; } = new RoutedCommand("ToggleTopmost", typeof(Commands));

    public static void Register(Type type, RoutedCommand command, ExecutedRoutedEventHandler executed) {
      CommandManager.RegisterClassCommandBinding(type, new CommandBinding(command, executed));
    }
  }

}
