using StausSaver.Maui.Pages;

namespace StausSaver.Maui
{
    public partial class App : Application
    {
        public App(AppShell appShell)
        {
            InitializeComponent();
            MainPage = appShell;

            Routing.RegisterRoute(nameof(ImageViewerPage), typeof(ImageViewerPage));
            Routing.RegisterRoute(nameof(VideoPlayerPage), typeof(VideoPlayerPage));
        }
    }
}