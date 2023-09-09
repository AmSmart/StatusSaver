using CommunityToolkit.Maui.Core.Extensions;
using StausSaver.Maui.Services;
using StausSaver.Maui.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusSaver.Maui.ViewModels;

public partial class VideosViewModel : ViewModelBase
{
    private readonly MediaService _mediaService;

    [ObservableProperty]
    private ObservableCollection<string> _videoThumbnails;

    public VideosViewModel(MediaService mediaService)
    {
        _mediaService = mediaService;
        var stopwatch = new Stopwatch();

        stopwatch.Start();
        _videoThumbnails = _mediaService.GetWhatsappMedia(MediaType.Video)
            .AsParallel()
            .Select(x => x.ToString())
            //.Select(x => _mediaService.GetThumbnailImageSourceFromVideo(x, 50)).ToObservableCollection();
            .Select(x => _mediaService.GetThumbnailAsPathFromVideo(x, 50, FileSystem.Current.CacheDirectory)).ToObservableCollection();
        stopwatch.Stop();

        Console.WriteLine($"Time elapsed to load videos: {stopwatch.ElapsedMilliseconds}ms");
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
