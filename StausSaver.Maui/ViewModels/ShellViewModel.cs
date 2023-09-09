using StatusSaver.Maui.Resources.Strings;
using StausSaver.Maui.Models;
using StausSaver.Maui.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StausSaver.Maui.ViewModels;

public class ShellViewModel : ViewModelBase
{
    public AppSection Images { get; set; }
    public AppSection Videos { get; set; }
    public AppSection Gallery { get; set; }
    public AppSection ImageViewer { get; set; }
    public AppSection VideoPlayer { get; set; }

    public ShellViewModel()
	{
        Images = new() { Title = AppStrings.ImagesPageTitle, Route = nameof(ImagesPage), Icon = "", IconDark = "", TargetType = typeof(ImagesPage) };
        Videos = new() { Title = AppStrings.VideosPageTitle, Route = nameof(VideosPage), Icon = "", IconDark = "", TargetType = typeof(VideosPage) };
        Gallery = new() { Title = AppStrings.GalleryPageTitle, Route = nameof(GalleryPage), Icon = "", IconDark = "", TargetType = typeof(GalleryPage) };
        ImageViewer = new() { Title = AppStrings.ImageViewerPageTitle, Route = nameof(ImageViewerPage), Icon = "", IconDark = "", TargetType = typeof(ImageViewerPage) };
        VideoPlayer = new() { Title = AppStrings.VideoPlayerPageTitle, Route = nameof(VideoPlayerPage), Icon = "", IconDark = "", TargetType = typeof(VideoPlayerPage) };
	}
}
