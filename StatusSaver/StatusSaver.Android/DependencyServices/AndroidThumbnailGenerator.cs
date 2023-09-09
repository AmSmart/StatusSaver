using Android.Graphics;
using Android.Media;
using StatusSaver.DependencyServices;
using StatusSaver.Droid.DependencyServices;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidThumbnailGenerator))]
namespace StatusSaver.Droid.DependencyServices
{
    public class AndroidThumbnailGenerator : IThumbnailGenerator
    {
        public ImageSource GenerateThumbnailImageSource(string url, long usecond)
        {
            // var bitmap = ThumbnailUtils.CreateVideoThumbnail(url, Android.Provider.ThumbnailKind.MicroKind);
            // above implementation seems slower

            MediaMetadataRetriever retriever = new MediaMetadataRetriever();
            retriever.SetDataSource(url);
            Bitmap bitmap = retriever.GetFrameAtTime(usecond);
            if (bitmap != null)
            {
                MemoryStream stream = new MemoryStream();
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                byte[] bitmapData = stream.ToArray();
                return ImageSource.FromStream(() => new MemoryStream(bitmapData));
            }
            return null;
        }

        public string GenerateThumbnailAsPath(string url, long usecond, string appCachePath)
        {

            MediaMetadataRetriever retriever = new MediaMetadataRetriever();
            retriever.SetDataSource(url);
            Bitmap bitmap = retriever.GetFrameAtTime(usecond);
            if (bitmap != null)
            {
                int slashLastIndex = url.LastIndexOf('/');
                int dotLastIndex = url.LastIndexOf('.');
                int fileNameLength = url.Length - (url.Length - dotLastIndex) - slashLastIndex - 1;
                string fileName = url.Substring(slashLastIndex + 1, fileNameLength);
                string outputPath = System.IO.Path.Combine(appCachePath, fileName + ".png");
                FileStream stream = new FileStream(outputPath, FileMode.CreateNew);
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                stream.Flush();
                stream.Close();
                return outputPath;
            }
            return null;

        }
    }
}
