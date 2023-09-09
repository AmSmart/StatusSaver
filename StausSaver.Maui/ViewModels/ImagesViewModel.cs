using CommunityToolkit.Maui.Core.Extensions;
using StausSaver.Maui.Services;
using StausSaver.Maui.ViewModels;
using StausSaver.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
