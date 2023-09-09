using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.DocumentFile.Provider;
using CommunityToolkit.Mvvm.Messaging;
using StatusSaver.Maui.Messages;
using Uri = Android.Net.Uri;

namespace StausSaver.Maui
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity { }
}