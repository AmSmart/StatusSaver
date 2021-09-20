using MvvmHelpers;
using StatusSaver.DependencyServices;
using StatusSaver.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StatusSaver.ViewModels
{
    public class PermissionsRequestedViewModel : BaseViewModel
    {
        private readonly IPathManager _pathManager;
        private bool _permissionsGranted;

        public PermissionsRequestedViewModel(IPathManager pathManager)
        {
            GrantPermissions = new Command(async () => await OnGrantPermissions());
            _pathManager = pathManager;
        }

        public ICommand GrantPermissions { get; set; }

        public bool PermissionsGranted
        {
            get => _permissionsGranted;
            set => SetProperty(ref _permissionsGranted, value);
        }

        async Task OnGrantPermissions()
        {
            var readPermissionStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            var writePermissionStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();

            var manageAllAccess = _pathManager.GetAllFilesAccess();

            if (readPermissionStatus != PermissionStatus.Granted)
            {
                readPermissionStatus = await Permissions.RequestAsync<Permissions.StorageRead>();
            }

            if (writePermissionStatus != PermissionStatus.Granted)
            {
                writePermissionStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();
            }


            if (readPermissionStatus == PermissionStatus.Granted && writePermissionStatus == PermissionStatus.Granted
                && manageAllAccess)
            {
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                Application.Current.MainPage = new PermissionsDeniedPage();
            }

        }
    }
}
