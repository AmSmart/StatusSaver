using CommunityToolkit.Maui;
using StatusSaver.Maui.ViewModels;
using StausSaver.Maui.Pages;
using StausSaver.Maui.Services;

namespace StausSaver.Maui;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMediaElement()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.ConfigurePages()
            .ConfigureServices()
            .ConfigureViewModels();

        return builder.Build();
    }
}