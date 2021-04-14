using StatusSaver.DependencyServices;
using StatusSaver.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StatusSaver.ServicesConcrete
{
    public class MediaManager : IMediaManager
    {
        private const string DefaultFileName = "file";

        private readonly string _appStoragePath;
        private readonly string _appImagesStoragePath;
        private readonly string _appVideosStoragePath;
        private readonly string _appCachePath;
        private readonly IVideoJoiner _videoJoiner;

        public MediaManager(IPathManager pathManager, IVideoJoiner videoJoiner)
        {
            _appStoragePath = pathManager.GetAppStoragePath();
            _appImagesStoragePath = pathManager.GetImagesStoragePath();
            _appVideosStoragePath = pathManager.GetVideosStoragePath();
            _appCachePath = pathManager.GetAppCachePath();

            InitialiseDirectories();
            
            _videoJoiner = videoJoiner;
        }

        public void SaveSingle(string path, FileType type, string name = DefaultFileName)
        {
            string storagePath = "";

            switch (type)
            {
                case FileType.Image:
                    storagePath = _appImagesStoragePath;
                    break;

                case FileType.Video:
                    storagePath = _appVideosStoragePath;
                    break;
            }

            string newName = $"{name}-{DateTime.Now.ToString("yyyyMMddTHH-mm-ss")}{Path.GetExtension(path)}";
            string newPath = Path.Combine(storagePath, newName);

            File.Copy(path, newPath);
        }

        public void SaveMultiple(string[] paths, FileType type, string name = DefaultFileName)
        {
            int counter = 0;

            string storagePath = "";            

            switch (type)
            {
                case FileType.Image:
                    storagePath = _appImagesStoragePath;
                    break;

                case FileType.Video:
                    storagePath = _appVideosStoragePath;
                    break;
            }

            foreach (string path in paths)
            {
                string newName = $"{name}{counter}-{DateTime.Now.ToString("yyyyMMddTHH-mm-ss")}{Path.GetExtension(path)}";
                string newPath = Path.Combine(storagePath, newName);

                File.Copy(path, newPath);

                counter++;
            }
        }



        public void SaveVideosAsOne(string[] paths, string name = DefaultFileName)
        {
            string dateTime = DateTime.Now.ToString("yyyyMMddTHH-mm-ss");
            string outputVideoName = $"{name}-{dateTime}{Path.GetExtension(paths[0])}";
            string outputVideoPath = Path.Combine(_appVideosStoragePath, outputVideoName);
            _videoJoiner.MergeVideos(paths, outputVideoPath);            
        }

        private void InitialiseDirectories()
        {
            if (!Directory.Exists(_appStoragePath))
            {
                Directory.CreateDirectory(_appStoragePath);
            }

            if (!Directory.Exists(_appImagesStoragePath))
            {
                Directory.CreateDirectory(_appImagesStoragePath);
            }

            if (!Directory.Exists(_appVideosStoragePath))
            {
                Directory.CreateDirectory(_appVideosStoragePath);
            }

            if (!Directory.Exists(_appCachePath))
            {
                Directory.CreateDirectory(_appCachePath);
            }

            ClearAppCache();

        }

        public void ClearAppCache()
        {
            DirectoryInfo di = new DirectoryInfo(_appCachePath);
            var files = di.GetFiles();

            foreach (FileInfo file in files)
            {
                file.Delete();
            }
        }
    }
}