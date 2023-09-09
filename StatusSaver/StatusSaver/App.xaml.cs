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
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            //if (VersionTracking.IsFirstLaunchEver)
            //{
            //    MainPage = new PermissionsRequestPage();
            //    return;
            //}

            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
