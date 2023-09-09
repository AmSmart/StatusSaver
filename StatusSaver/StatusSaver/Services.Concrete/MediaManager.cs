using StatusSaver.DependencyServices;
using StatusSaver.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections.ObjectModel;
using StatusSaver.Models;

namespace StatusSaver.ServicesConcrete
{
    public class MediaManager : IMediaManager
    {
        private const string DefaultFileName = "file";

        private readonly string _appStoragePath;
        private readonly string _appImagesStoragePath;
        private readonly string _appVideosStoragePath;
        private readonly string _appCachePath;
        private readonly IEnumerable<string> _statusResourcesPaths;
        private readonly IVideoJoiner _videoJoiner;
        private readonly IThumbnailGenerator _thumbnailGenerator;

        public MediaManager(IPathManager pathManager, IVideoJoiner videoJoiner,
            IThumbnailGenerator thumbnailGenerator)
        {
            _appStoragePath = pathManager.GetAppStoragePath();
            _appImagesStoragePath = pathManager.GetImagesStoragePath();
            _appVideosStoragePath = pathManager.GetVideosStoragePath();
            _appCachePath = pathManager.GetAppCachePath();
            _statusResourcesPaths = pathManager.GetStatusResourcesPaths();
            _thumbnailGenerator = thumbnailGenerator;
            _videoJoiner = videoJoiner;

            InitialiseDirectories();            
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

        public void LoadImages(ObservableCollection<Image> images)
        {
            List<string> allImageUrls = new List<string>();

            foreach (var path in _statusResourcesPaths)
            {
                if (Directory.Exists(path))
                {
                    var files = Directory.GetFiles(path).Where(x => x.EndsWith(".jpg"));
                    allImageUrls.AddRange(files);
                }
            }

            images.Clear();
            foreach (string url in allImageUrls)
            {
                images.Add(new Image
                {
                    Path = url
                });
            }
        }

        public void LoadVideos(ObservableCollection<Video> videos)
        {
            List<string> allVideoUrls = new List<string>();
            foreach (var path in _statusResourcesPaths)
            {
                if (Directory.Exists(path))
                {
                    var files = Directory.GetFiles(path).Where(x => x.EndsWith(".mp4"));
                    allVideoUrls.AddRange(files);
                }
            }

            videos.Clear();
            foreach (string url in allVideoUrls)
            {
                videos.Add(new Video
                {
                    ImageCachePath = _thumbnailGenerator.GenerateThumbnailAsPath(url, 1000, _appCachePath),
                    ImageSource = null,/*thumbnailGenerator.GenerateThumbnail(url, 1000),*/
                    Path = url,
                });
            }
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