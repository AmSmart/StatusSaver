using CommunityToolkit.Maui;
using StatusSaver.Maui.ViewModels;
using StausSaver.Maui.Pages;
using StausSaver.Maui.Services;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace StausSaver.Maui;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.ConfigurePages()
            .ConfigureServices()
            .ConfigureViewModels();

//#if ANDROID
//        ImageHandler.Mapper.PrependToMapping(nameof(Microsoft.Maui.IImage.Source), (handler, view) =>
//        {
//            handler.PlatformView.Clear();
//        });
//#endif

        return builder.Build();
    }
}