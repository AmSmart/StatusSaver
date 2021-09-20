using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StatusSaver.Services;
using StatusSaver.Views;
using StatusSaver.ViewModels;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.Threading;

namespace StatusSaver
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            Startup.Initialise();            
            Device.SetFlags(new string[] { "CarouselView_Experimental" });
        }

        protected override async void OnStart()
        {
            if (VersionTracking.IsFirstLaunchEver)
            {
                MainPage = new PermissionsRequestPage();
                return;
            }

            var permissionStatus = await CheckPermissionStatus();
            
            if (permissionStatus)
            {
                MainPage = new AppShell();
                return;
            }

            MainPage = new PermissionsDeniedPage();
        }

        protected override void OnSleep()
        {
        }

        protected override async void OnResume()
        {
            var permissionGranted = await CheckPermissionStatus();

            switch (Current.MainPage)
            {
                case AppShell shell when !permissionGranted:
                    Current.MainPage = new PermissionsDeniedPage();
                    return;

                case PermissionsDeniedPage page when permissionGranted:
                    Current.MainPage = new AppShell();
                    return;

                default:
                    return;
            }
        }

        private async Task<bool> CheckPermissionStatus()
        {
            var readPermissionStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            var writePermissionStatus = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

            if(readPermissionStatus == PermissionStatus.Granted && writePermissionStatus == PermissionStatus.Granted)
            {
                return true;
            }
            return false;
        }
    }
}
