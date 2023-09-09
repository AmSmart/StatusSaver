using Android.Content;
using AndroidX.DocumentFile.Provider;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.Messaging;
using StatusSaver.Maui.Messages;
using StatusSaver.Maui.Services;
using StausSaver.Maui;
using StausSaver.Maui.Pages;
using StausSaver.Maui.Services;
using StausSaver.Maui.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StatusSaver.Maui.ViewModels;

public partial class PermissionsViewModel : ViewModelBase
{
    private readonly MediaService _mediaService;
    private readonly ToastService _toastService;

    public PermissionsViewModel(MediaService mediaService, ToastService toastService)
    {
        _mediaService = mediaService;
        _toastService = toastService;
    }

    [RelayCommand]
    async Task AllowAccess()
    {

        IsBusy = true;

        await _mediaService.RequestStoragePermissions();
        var folderURI = await _mediaService.RequestMediaFolderAccess();
        string folderURIString = folderURI.ToString();

        if (string.IsNullOrEmpty(folderURIString) || !folderURIString.EndsWith(""))
        {
            await _toastService.ShowShortToast("Permission not granted to requested folder");
            IsBusy = false;
            return;
        }


        Settings.MediaFolderUri = folderURI.ToString();
        await Shell.Current.GoToAsync($"///{nameof(ImagesPage)}");
        IsBusy = false;
    }

    // TODO: Remove this method and it's references
    public void RegisterPermissionCompleteMessage()
    {
        StrongReferenceMessenger.Default.Register<PermissionRequestCompletedMessage>(this, (r, m) =>
        {
            bool accessAllowed = false;
            // TODO: Check if permission was granted

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (accessAllowed)
                {
                    await Shell.Current.GoToAsync($"///{nameof(ImagesPage)}");
                }
                else
                {
                    string text = "Premission not granted to requested folder";
                    await _toastService.ShowShortToast(text);
                }
            });
        });
    }

    // TODO: Remove this method and it's references
    public void UnregisterPermissionCompleteMessage()
    {
        StrongReferenceMessenger.Default.Unregister<PermissionRequestCompletedMessage>(this);
    }
}