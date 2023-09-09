using StatusSaver.Maui.ViewModels;

namespace StausSaver.Maui.Pages;

public partial class ImagesPage : ContentPage
{
	public ImagesPage(ImagesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}