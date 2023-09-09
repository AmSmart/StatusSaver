using StatusSaver.Maui.Services;
using StatusSaver.Maui.Services.MediaService;

namespace StausSaver.Maui.Services;

public static class ServiceExtensions
{
    public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<MediaService>();
        builder.Services.AddSingleton<ToastService>();

        return builder;
    }
}
