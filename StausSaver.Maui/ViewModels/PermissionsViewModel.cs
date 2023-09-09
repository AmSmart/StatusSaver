using StatusSaver.Maui.Services;
using StatusSaver.Maui.Services.MediaService;
using StausSaver.Maui;
using StausSaver.Maui.Pages;
using StausSaver.Maui.ViewModels;

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
        string folderURIString;
        string errorMessage = "Permission not granted to requested folder";

        try
        {
            IsBusy = true;

            await _mediaService.RequestStoragePermissions();
            var folderURI = await _mediaService.RequestMediaFolderAccess();
            folderURIString = folderURI.ToString();

            if (!string.IsNullOrEmpty(folderURIString) && folderURIString.EndsWith(""))
            {
                Settings.MediaFolderUri = folderURIString;
                await Shell.Current.GoToAsync($"///{nameof(ImagesPage)}");
                IsBusy = false;
            }
            else
            {
                await _toastService.ShowShortToast(errorMessage);
                IsBusy = false;
            }
        }
        catch (Exception ex)
        {
            await _toastService.ShowShortToast(errorMessage);
            IsBusy = false;
        }
    }
}