using Android.OS;
using StatusSaver.Droid.DependencyServices;
using StatusSaver.DependencyServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;
using Android.Support.V4.App;
using Android;
using Android.Content;
using Xamarin.Essentials;
using Android.Provider;

[assembly: Dependency(typeof(AndroidPathManager))]
namespace StatusSaver.Droid.DependencyServices
{    
    public class AndroidPathManager : IPathManager
    {
        private const string AppName = "Smart's Status Saver";
        private const string Images = "Images";
        private const string Videos = "Videos";
        private const string Cache = ".cache";
        private const string WhatAppResourceDirectory = "Android/media/com.whatsapp";
        private const string WhatsAppStatusDirectory = "WhatsApp/Media/.Statuses";
        private const string WhatsAppBusinessAndroidStatusDirectory = "WhatsApp Business/Media/.Statuses";

        private readonly string _deviceStorageDirectory;
        private readonly string _appStoragePath;
        private readonly string _appImagesStoragePath;
        private readonly string _appVideosStoragePath;
        private readonly string _appCachePath;


        public AndroidPathManager()
        {
            if(Android.OS.Build.VERSION.SdkInt >= BuildVersionCodes.R)
            {
                if (!Environment.IsExternalStorageManager)
                    (Application.Current.MainPage as Shell).GoToAsync("//requestpermission", true);
            }
            else
            {
                // TODO: Add permission for lower android versions
            }

            _deviceStorageDirectory = Path.Combine(Environment.StorageDirectory.AbsolutePath,"emulated/0");

            _appStoragePath = Platform.CurrentActivity.GetExternalMediaDirs()[0].AbsolutePath;
            _appCachePath = Path.Combine(_appStoragePath, Cache);
            _appImagesStoragePath = Path.Combine(_appStoragePath, Images);
            _appVideosStoragePath = Path.Combine(_appStoragePath, Videos);

            WhatsAppStatusResourcesPaths = new List<string>
            {
                Path.Combine(_deviceStorageDirectory,WhatAppResourceDirectory,WhatsAppStatusDirectory),
                Path.Combine(_deviceStorageDirectory,WhatsAppStatusDirectory),
                Path.Combine(_deviceStorageDirectory,WhatAppResourceDirectory,WhatsAppBusinessAndroidStatusDirectory),
                Path.Combine(_deviceStorageDirectory,WhatsAppBusinessAndroidStatusDirectory)
            };            
        }

        public List<string> WhatsAppStatusResourcesPaths { get; set; }

        public string GetDeviceStoragePath()
            => _deviceStorageDirectory;

        public string GetAppStoragePath()
            => _appStoragePath;

        public string GetImagesStoragePath()
            => _appImagesStoragePath;

        public string GetVideosStoragePath()
            => _appVideosStoragePath;

        public IEnumerable<string> GetStatusResourcesPaths()
            => WhatsAppStatusResourcesPaths;

        public string GetAppCachePath()
            => _appCachePath;

        public bool CheckPathAccess() => Environment.IsExternalStorageManager;

        public void RequestPathAccess() 
            => Platform.CurrentActivity.StartActivityForResult(new Intent(Android.Provider.Settings.ActionManageAllFilesAccessPermission), 3);
    }
}
