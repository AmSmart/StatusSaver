using StatusSaver.Maui.ViewModels;

namespace StausSaver.Maui.Pages;

public partial class VideosPage : ContentPage
{
	public VideosPage(VideosViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}