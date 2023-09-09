using CommunityToolkit.Maui.Core.Extensions;
using StausSaver.Maui.ViewModels;
using StatusSaver.Maui.Services.MediaService;
using StausSaver.Maui.Pages;

namespace StatusSaver.Maui.ViewModels;

public partial class ImagesViewModel : ViewModelBase
{
    private readonly MediaService _mediaService;

    [ObservableProperty]
    private SelectionMode _selectionMode;

    [ObservableProperty]
    private ObservableCollection<string> _imageUris;

    public ImagesViewModel(MediaService mediaService)
    {
        _mediaService = mediaService;
        _selectionMode = SelectionMode.None;
        _imageUris = _mediaService.GetWhatsappMedia(MediaType.Image)
            .Select(x => x.ToString())
            .ToObservableCollection();
    }

    [RelayCommand]
    async void RefreshList()
    {
        IsBusy = true;
        await Task.Delay(3000);
        IsBusy = false;
    }

    [RelayCommand]
    void HandleSelectionChanged()
    {

    }
    
    [RelayCommand]
    async void ViewImage(string selectedImageUri)
    {
        var navigationParameters = new Dictionary<string, object>
        {
            { nameof(ImageUris),  ImageUris },
            { "CurrentImageUri", selectedImageUri }
        };
        await Shell.Current.GoToAsync(nameof(ImageViewerPage), navigationParameters);
    }
}
