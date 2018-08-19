using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using CefSharp;

namespace Zabrodin.YouTubeTVPlayer
{
    public partial class MainWindow
    {
        private bool isFullScreen;
        private WindowState previousWindowState;
        private NotifyIcon trayIcon;

        public MainWindow()
        {
            ApplicationCommands.ChangeVisibilityCommand.InputGestures.Add(new KeyGesture(Key.V, ModifierKeys.Control | ModifierKeys.Shift));
            ApplicationCommands.ChangeVisibilityCommand.InputGestures.Add(new MouseGesture(MouseAction.LeftClick));

            ApplicationCommands.MaximizeCommand.InputGestures.Add(new KeyGesture(Key.F, ModifierKeys.Control));
            ApplicationCommands.MaximizeCommand.InputGestures.Add(new MouseGesture(MouseAction.LeftClick));

            ApplicationCommands.FullScreenCommand.InputGestures.Add(new KeyGesture(Key.F, ModifierKeys.Control | ModifierKeys.Shift));
            ApplicationCommands.FullScreenCommand.InputGestures.Add(new MouseGesture(MouseAction.LeftClick));

            ApplicationCommands.CloseApplicationCommand.InputGestures.Add(new KeyGesture(Key.X, ModifierKeys.Control | ModifierKeys.Shift));
            ApplicationCommands.CloseApplicationCommand.InputGestures.Add(new MouseGesture(MouseAction.LeftClick));

            var settings = new CefSettings
            {
                CachePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cache")
            };
            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

            InitializeComponent();

            Loaded += OnLoaded;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Cef.Shutdown();
            trayIcon.Dispose();

            base.OnClosing(e);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;

            InitializeTrayIcon();
            Browser.Address = "http://www.youtube.com/tv";
        }

        private void ChangeVisibility()
        {
            if (isFullScreen)
                return;

            if (Opacity.Equals(0))
            {
                ShowInTaskbar = true;
                Opacity = 1;
            }
            else
            {
                ShowInTaskbar = false;
                Opacity = 0;
            }

            Activate();
            Browser.Focus();
        }

        private void InitializeTrayIcon()
        {
            trayIcon = new NotifyIcon
            {
                Icon = Properties.Resources.YouTubeIcon
            };

            trayIcon.MouseDoubleClick += OnTrayIconDoubleClick;
            trayIcon.Visible = true;
        }

        private void OnChangeVisibilityCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ChangeVisibility();
        }

        private void OnMaximizeCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (isFullScreen)
                return;

            WindowState = WindowState != WindowState.Maximized ? WindowState.Maximized : WindowState.Normal;
        }

        private void OnFullScreenCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (isFullScreen)
            {
                isFullScreen = false;
                WindowState = previousWindowState;
                TitleBar.Visibility = Visibility.Visible;
                Browser.Focus();
            }
            else
            {
                previousWindowState = WindowState;
                WindowState = WindowState.Maximized;
                TitleBar.Visibility = Visibility.Collapsed;
                isFullScreen = true;
                Browser.Focus();
            }
        }

        private void OnTrayIconDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ChangeVisibility();
        }

        private void OnCloseApplicationCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}