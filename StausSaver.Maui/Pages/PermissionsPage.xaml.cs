using StatusSaver.Maui.ViewModels;
using StausSaver.Maui.Services;

namespace StausSaver.Maui.Pages;

public partial class PermissionsPage : ContentPage
{
    private readonly MediaService _mediaService;

    public PermissionsPage(PermissionsViewModel viewModel, MediaService mediaService)
	{
		InitializeComponent();
		BindingContext = viewModel;
        _mediaService = mediaService;
    }

    protected override async void OnAppearing()
    {
        if (Settings.MediaFolderUri is null) return;

        var accessAllowed = _mediaService.CheckMediaFolderAccess();
        if (accessAllowed)
            await Shell.Current.GoToAsync($"///{nameof(ImagesPage)}");
        
        base.OnAppearing();
    }
}