using MvvmHelpers;
using StatusSaver.DependencyServices;
using StatusSaver.ServicesAbstract;
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
    public class PermissionsRequestViewModel : BaseViewModel
    {
        private readonly IPathManager _pathManager;
        private readonly IPageManager _pageManager;
        private bool _permissionsGranted;

        public PermissionsRequestViewModel(IPathManager pathManager, IPageManager pageManager)
        {
            _pathManager = pathManager;
            _pageManager = pageManager;
            GrantPermissions = new Command(() => _pathManager.RequestPathAccess());
        }

        public ICommand GrantPermissions { get; set; }

        public bool PermissionsGranted
        {
            get => _permissionsGranted;
            set => SetProperty(ref _permissionsGranted, value);
        }

        public bool CheckAccessGranted() => _pathManager.CheckPathAccess();

        public void NavigateHome() => _pageManager.NavigateTo("//home");

        async Task OnGrantPermissions()
        {
            var readPermissionStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            var writePermissionStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();

            if (readPermissionStatus != PermissionStatus.Granted)
            {
                readPermissionStatus = await Permissions.RequestAsync<Permissions.StorageRead>();
            }

            if (writePermissionStatus != PermissionStatus.Granted)
            {
                writePermissionStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();
            }


            if (readPermissionStatus == PermissionStatus.Granted && writePermissionStatus == PermissionStatus.Granted)
            {
                Application.Current.MainPage = new AppShell();
            }

        }
    }
}
