using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StatusSaver.DependencyServices
{
    public interface IThumbnailGenerator
    {
        ImageSource GenerateThumbnailImageSource(string url, long usecond);
        string GenerateThumbnailAsPath(string url, long usecond, string appCachePath);
    }
}
