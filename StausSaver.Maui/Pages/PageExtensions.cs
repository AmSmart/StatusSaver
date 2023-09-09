using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StausSaver.Maui.Pages;

public static class PageExtensions
{
    public static MauiAppBuilder ConfigurePages(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<ImagesPage>();
        builder.Services.AddSingleton<VideosPage>();
        builder.Services.AddSingleton<GalleryPage>();
        
        builder.Services.AddTransient<ImageViewerPage>();
        builder.Services.AddTransient<VideoPlayerPage>();
        builder.Services.AddTransient<PermissionsPage>();

        return builder;
    }
}
