using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusSaver.DependencyServices
{
    public interface IPathManager
    {
        string GetDeviceStoragePath();
        string GetAppStoragePath();
        string GetAppCachePath();
        string GetImagesStoragePath();
        string GetVideosStoragePath();
        IEnumerable<string> GetStatusResourcesPaths();
        bool GetAllFilesAccess();
    }
}
