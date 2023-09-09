using CommunityToolkit.Maui.Core.Extensions;
using StatusSaver.Maui.Services.MediaService;
using StausSaver.Maui.Pages;
using StausSaver.Maui.ViewModels;

namespace StatusSaver.Maui.ViewModels;

public partial class VideosViewModel : ViewModelBase
{
    private readonly MediaService _mediaService;

    [ObservableProperty]
    private SelectionMode _selectionMode;

    [ObservableProperty]
    private ObservableCollection<Tuple<string, string>> _videoUris;

    public VideosViewModel(MediaService mediaService)
    {
        // TODO: Experiment fastest way to load and display thumbnails
        _mediaService = mediaService;

        _selectionMode = SelectionMode.None;
        _videoUris = _mediaService.GetWhatsappMedia(MediaType.Video)
            .AsParallel()
            .Select(x => x.ToString())
            .Select(x => Tuple.Create(x, _mediaService.GetThumbnailAsPathFromVideo(x, 50, FileSystem.Current.CacheDirectory)))
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
    async Task PlayVideo(string selectedVideoUri)
    {
        var navigationParameters = new Dictionary<string, object>
        {
            { "VideoUris",  VideoUris.Select(x => x.Item1).ToObservableCollection()},
            { "CurrentVideoUri", selectedVideoUri}
        };
        await Shell.Current.GoToAsync(nameof(VideoPlayerPage), navigationParameters);
    }

    [RelayCommand]
    void HandleSelectionChanged()
    {

    }
}
