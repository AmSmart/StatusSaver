using StatusSaver.Maui.ViewModels;

namespace StausSaver.Maui.Pages;

public partial class ImageViewerPage : ContentPage
{
	public ImageViewerPage(ImageViewerViewModel imageViewerViewModel)
	{
		InitializeComponent();
		BindingContext = imageViewerViewModel;
	}
}