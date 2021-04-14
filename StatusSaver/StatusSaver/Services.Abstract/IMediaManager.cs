using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace StatusSaver.Services.Abstract
{
    public interface IMediaManager
    {
        void SaveSingle(string path, FileType type, string name = "file");

        void SaveMultiple(string[] paths, FileType type, string name = "file");

        void SaveVideosAsOne(string[] paths, string name = "video");
        void ClearAppCache();
    }

    public enum FileType
    {
        Image,
        Video
    }
}
