using StatusSaver.Maui.ViewModels;

namespace StausSaver.Maui.Pages;

public partial class VideoPlayerPage : ContentPage
{
	public VideoPlayerPage(VideoPlayerViewModel videoPlayerViewModel)
	{
		InitializeComponent();
		BindingContext = videoPlayerViewModel;
	}
}