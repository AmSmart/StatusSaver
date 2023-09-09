using StausSaver.Maui.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusSaver.Maui.ViewModels;

public static class ViewModelExtensions
{
    public static MauiAppBuilder ConfigureViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<ShellViewModel>();
        builder.Services.AddSingleton<ImagesViewModel>();
        builder.Services.AddSingleton<VideosViewModel>();
        builder.Services.AddSingleton<GalleryViewModel>();
        
        builder.Services.AddTransient<PermissionsViewModel>();
        builder.Services.AddTransient<ImageViewerViewModel>();
        builder.Services.AddTransient<VideoPlayerViewModel>();
        return builder;
    }
}
