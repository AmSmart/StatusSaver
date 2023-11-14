using StatusSaver.Maui.ViewModels;

namespace StausSaver.Maui.Pages;

public partial class VideoPlayerPage : ContentPage
{
	public VideoPlayerPage(VideoPlayerViewModel videoPlayerViewModel)
	{
		InitializeComponent();
		BindingContext = videoPlayerViewModel;
	}

    private void ContentPage_Unloaded(object sender, EventArgs e)
    {
		mediaElement.Handler?.DisconnectHandler();
    }
}