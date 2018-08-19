using System.Windows.Input;

namespace Zabrodin.YouTubeTVPlayer
{
    public static class ApplicationCommands
    {
        public static RoutedCommand ChangeVisibilityCommand = new RoutedCommand();
        public static RoutedCommand MaximizeCommand = new RoutedCommand();
        public static RoutedCommand FullScreenCommand = new RoutedCommand();
        public static RoutedCommand CloseApplicationCommand = new RoutedCommand();
    }
}
