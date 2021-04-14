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

[assembly: Dependency(typeof(PathManagerAndroid))]
namespace StatusSaver.Droid.DependencyServices
{    
    public class PathManagerAndroid : IPathManager
    {
        private const string AppName = "Smart's Status Saver";
        private const string Images = "Images";
        private const string Videos = "Videos";
        private const string Cache = ".cache";
        private const string WhatsAppAndroidStatusDirectory = "WhatsApp/Media/.Statuses";

        private readonly string _deviceStorageDirectory;
        private readonly string _appStoragePath;
        private readonly string _appImagesStoragePath;
        private readonly string _appVideosStoragePath;
        private readonly string _statusResourcesPath;
        private readonly string _appCachePath;


        public PathManagerAndroid()
        {            
            _deviceStorageDirectory = Environment.ExternalStorageDirectory.AbsolutePath;
            _appStoragePath = Path.Combine(_deviceStorageDirectory, AppName);
            _appCachePath = Path.Combine(_appStoragePath, Cache);
            _appImagesStoragePath = Path.Combine(_appStoragePath, Images);
            _appVideosStoragePath = Path.Combine(_appStoragePath, Videos);
            _statusResourcesPath = Path.Combine(_deviceStorageDirectory,WhatsAppAndroidStatusDirectory);
        }

        public string GetDeviceStoragePath()
            => _deviceStorageDirectory;

        public string GetAppStoragePath()
            => _appStoragePath;

        public string GetImagesStoragePath()
            => _appImagesStoragePath;

        public string GetVideosStoragePath()
            => _appVideosStoragePath;

        public string GetStatusResourcesPath()
            => _statusResourcesPath;

        public string GetAppCachePath()
            => _appCachePath;
    }
}
