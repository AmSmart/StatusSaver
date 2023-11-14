using Android.Content;
using AndroidX.DocumentFile.Provider;
using Directory = System.IO.Directory;
using Uri = Android.Net.Uri;
using Com.Googlecode.Mp4parser.Authoring;
using Com.Googlecode.Mp4parser.Authoring.Container.Mp4;
using Com.Googlecode.Mp4parser;
using Com.Googlecode.Mp4parser.Authoring.Builder;
using Com.Googlecode.Mp4parser.Authoring.Tracks;
using Android.Media;
using Java.Lang;
using Java.Nio;
using Java.IO;
using Java.Nio.Channels;
using Movie = Com.Googlecode.Mp4parser.Authoring.Movie;
using CommunityToolkit.Maui.Storage;
using System.Web;
using Android.Graphics;
using Path = System.IO.Path;
using StausSaver.Maui;

namespace StatusSaver.Maui.Services.MediaService;

public class MediaService
{
    const string AppName = "com.smart.whatsapptools";
    const string MediaFolderPath = "Android/Media";
    const string UrlEncodedMediaFolderPath = "Android%2FMedia";
    const string BaseURI = "content://com.android.externalstorage.documents/tree/primary%3A";
    const int DefaultBufferSize = 1 * 1024 * 1024;

    Context _context;
    string _deviceMediaPath;
    string _appMediaPath;


    public MediaService()
    {
        _context = Android.App.Application.Context;
        _deviceMediaPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath,
            MediaFolderPath);
        _appMediaPath = Path.Combine(_deviceMediaPath, AppName);
    }

    public async Task RequestStoragePermissions()
    {
        // TODO: Only if device is less than Android 13
        var readPermissionStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
        var writePermissionStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();
        var mediaPermissionStatus = await Permissions.CheckStatusAsync<Permissions.Media>();

        if (readPermissionStatus != PermissionStatus.Granted)
        {
            readPermissionStatus = await Permissions.RequestAsync<Permissions.StorageRead>();
        }

        if (writePermissionStatus != PermissionStatus.Granted)
        {
            writePermissionStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();
        }

        if (mediaPermissionStatus != PermissionStatus.Granted)
        {
            mediaPermissionStatus = await Permissions.RequestAsync<Permissions.Media>();
        }

        return;

    }

    public async Task<Uri> RequestMediaFolderAccess()
    {
        var cts = new CancellationTokenSource();
        var result = await FolderPicker.Default.PickAsync(MediaFolderPath, cts.Token);
        result.EnsureSuccess();
        var folder = result.Folder;

        if (!folder.Path.EndsWith("Android/Media"))
            throw new System.Exception("Invalid folder path");

        var contentResolver = Platform.AppContext.ContentResolver;
        var uri = Uri.Parse(BaseURI + UrlEncodedMediaFolderPath);
        contentResolver.TakePersistableUriPermission(uri, ActivityFlags.GrantWriteUriPermission);
        contentResolver.TakePersistableUriPermission(uri, ActivityFlags.GrantReadUriPermission);


        

        string folderPath = folder.Path.Replace("/storage/emulated/0/", string.Empty, StringComparison.InvariantCulture);
        var folderURI = Uri.Parse(BaseURI + HttpUtility.UrlEncode(folderPath));

        return folderURI;
    }

    public bool CheckMediaFolderAccess()
    {
        var c = Platform.AppContext.ContentResolver.PersistedUriPermissions.Select(x => x.Uri.ToString());
        return Platform.AppContext.ContentResolver.PersistedUriPermissions
            .Any(x => x.Uri.ToString().ToLower().EndsWith("android%2fmedia"));
    }

    public byte[] GetFileBytes(string uriString)
    {
        var uri = Uri.Parse(uriString);
        var stream = _context.ContentResolver.OpenInputStream(uri);
        using var memStream = new MemoryStream();
        stream.CopyTo(memStream);
        return memStream.ToArray();
    }

    public IEnumerable<Uri> GetWhatsappMedia(MediaType mediaType)
    {
        Func<DocumentFile, bool> filter;

        switch (mediaType)
        {
            case MediaType.Image:
                filter = x => x.Uri.ToString().EndsWith(".jpg");
                break;

            case MediaType.Video:
                filter = x => x.Uri.ToString().EndsWith(".mp4");
                break;

            case MediaType.All:
                filter = x => x.Uri.ToString().EndsWith(".jpg") || x.Uri.ToString().EndsWith(".mp4");
                break;

            default:
                throw new InvalidOperationException();
        }

        var mediaUri = Uri.Parse(Settings.MediaFolderUri);
        var mediaDir = DocumentFile.FromTreeUri(Platform.AppContext, mediaUri);

        var statuses = mediaDir.FindFile("com.whatsapp")
                ?.FindFile("WhatsApp")
                ?.FindFile("Media")
                ?.FindFile(".Statuses")
                ?.ListFiles()
                ?.Where(filter)
                ?.Select(X => X.Uri)
                ?? Enumerable.Empty<Uri>();

        return statuses;
    }

    public ImageSource GetThumbnailImageSourceFromVideo(string url, long usecond)
    {
        MediaMetadataRetriever retriever = new MediaMetadataRetriever();
        retriever.SetDataSource(_context, Uri.Parse(url));
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

    public string GetThumbnailAsPathFromVideo(string url, long usecond, string appCachePath)
    {

        MediaMetadataRetriever retriever = new MediaMetadataRetriever();
        retriever.SetDataSource(_context, Uri.Parse(url));
        Bitmap bitmap = retriever.GetFrameAtTime(usecond);
        if (bitmap != null)
        {
            string outputPath = Path.Combine(appCachePath, Guid.NewGuid().ToString() + ".png");
            FileStream stream = new FileStream(outputPath, FileMode.CreateNew);
            bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
            stream.Flush();
            stream.Close();
            return outputPath;
        }
        return null;

    }

    public void SaveMedia(byte[] data, MediaType mediaType, string fileName)
    {
        switch (mediaType)
        {
            case MediaType.Image:
                fileName += ".jpg";
                string imagesPath = Path.Combine(_deviceMediaPath, AppName, "Images");
                if (!Directory.Exists(imagesPath))
                    Directory.CreateDirectory(imagesPath);

                System.IO.File.WriteAllBytes(Path.Combine(imagesPath, fileName), data);
                break;

            case MediaType.Video:
                fileName += ".mp4";
                string videosPath = Path.Combine(_deviceMediaPath, AppName, "Videos");
                if (!Directory.Exists(videosPath))
                    Directory.CreateDirectory(videosPath);

                System.IO.File.WriteAllBytes(Path.Combine(videosPath, fileName), data);
                break;

            default:
                throw new InvalidOperationException();
        }
    }

    public void JoinAndSaveVideos(List<byte[]> data, string fileName)
    {
        fileName += ".mp4";
        string videosPath = Path.Combine(_deviceMediaPath, "Videos");
        if (!Directory.Exists(videosPath))
            Directory.CreateDirectory(videosPath);
        string filePath = Path.Combine(videosPath, fileName);

        MergeVideos(data, filePath);
    }

    public void SplitVideosAtInterval(Uri videoURI, int interval, string outputFolderName)
    {
        var stream = _context.ContentResolver.OpenInputStream(videoURI);
        using var memStream = new MemoryStream();
        stream.CopyTo(memStream);
        var data = memStream.ToArray();

        IDataSource source = new MemoryDataSourceImpl(data);
        var movie = MovieCreator.Build(source);

        int durationInSeconds = (int)(movie.Tracks[0].GetSampleDurations().Sum() / movie.Tracks[0].TrackMetaData.Timescale);
        int intervalsCount = durationInSeconds % interval == 0
            ? durationInSeconds / interval
            : durationInSeconds / interval + 1;

        var intervalMap = new List<(int, int)>();
        int currentKey = -1;
        int currentValue = interval - 1;

        int c = 0;
        while (currentValue <= durationInSeconds)
        {
            if (c == 0)
            {
                intervalMap.Add((currentKey, currentValue));
            }
            else
            {
                currentKey = currentKey + interval - 1;
                currentValue = currentValue + interval - 1;
                intervalMap.Add((currentKey, currentValue));
            }

            c++;
        }

        for (int i = 0; i < intervalMap.Count; i++)
        {
            string outputPath = Path.Combine(_appMediaPath, "Splits", outputFolderName, $"{i + 1}.mp4");
            SplitVideoWithMuxer(videoURI, outputPath, intervalMap[i].Item1 * 1000, intervalMap[i].Item2 * 1000);
            //SplitVideoWithMp4Parser(data, outputPath, intervalMap[i].Item1 * 1000, intervalMap[i].Item2 * 1000);
        }
    }

    private void MergeVideos(List<byte[]> videos, string outputPath)
    {
        List<Movie> inMovies = new List<Movie>();
        Movie movie = new Movie();

        foreach (var video in videos)
        {
            IDataSource source = new MemoryDataSourceImpl(video);
            var inMovie = MovieCreator.Build(source);
            inMovies.Add(inMovie);
        }

        List<List<ITrack>> allTracks = new List<List<ITrack>>();
        for (int i = 0; i < videos.Count; i++)
        {
            allTracks.Add(new List<ITrack>());
        }

        for (int i = 0; i < inMovies.Count(); i++)
        {
            foreach (ITrack track in inMovies[i].Tracks)
            {
                allTracks[i].Add(track);
            }
        }

        ITrack[] tracks = new ITrack[allTracks.Count()];
        for (int i = 0; i < allTracks[0].Count(); i++)
        {
            for (int j = 0; j < allTracks.Count(); j++)
            {
                tracks[j] = allTracks[j][i];
            }
            movie.AddTrack(new AppendTrack(tracks));
            tracks = new ITrack[allTracks.Count()];
        }


        BasicContainer oout = (BasicContainer)new DefaultMp4Builder().Build(movie);
        FileOutputStream fos = new FileOutputStream(new Java.IO.File(outputPath));
        oout.WriteContainer(fos.Channel);
        fos.Close();
    }

    private void SplitVideoWithMuxer(Uri srcUri, string outputPath, int startMs, int endMs)
    {
        // Set up MediaExtractor to read from the source.
        MediaExtractor extractor = new MediaExtractor();
        extractor.SetDataSource(_context, srcUri, null);

        int trackCount = extractor.TrackCount;

        // Set up MediaMuxer for the destination.
        MediaMuxer muxer;
        muxer = new MediaMuxer(outputPath, MuxerOutputType.Mpeg4);

        // Set up the tracks and retrieve the max buffer size for selected tracks.
        Dictionary<int, int> indexMap = new(trackCount);
        int bufferSize = -1;
        for (int i = 0; i < trackCount; i++)
        {
            MediaFormat format = extractor.GetTrackFormat(i);
            string mime = format.GetString(MediaFormat.KeyMime);
            bool selectCurrentTrack = false;
            if (mime.StartsWith("audio/"))
            {
                selectCurrentTrack = true;
            }
            else if (mime.StartsWith("video/"))
            {
                selectCurrentTrack = true;
            }
            if (selectCurrentTrack)
            {
                extractor.SelectTrack(i);
                int dstIndex = muxer.AddTrack(format);
                indexMap.Add(i, dstIndex);
                if (format.ContainsKey(MediaFormat.KeyMaxInputSize))
                {
                    int newSize = format.GetInteger(MediaFormat.KeyMaxInputSize);
                    bufferSize = newSize > bufferSize ? newSize : bufferSize;
                }
            }
        }
        if (bufferSize < 0)
        {
            bufferSize = DefaultBufferSize;
        }
        // Set up the orientation and starting time for extractor.
        MediaMetadataRetriever retrieverSrc = new MediaMetadataRetriever();
        // TODO: 
        retrieverSrc.SetDataSource(_context, srcUri);
        string degreesString = retrieverSrc.ExtractMetadata(MetadataKey.VideoRotation);
        try
        {
            retrieverSrc.Release();
        }
        catch (Java.IO.IOException)
        {
            // Ignore errors occurred while releasing the MediaMetadataRetriever.
        }
        if (degreesString != null)
        {
            int degrees = int.Parse(degreesString);
            if (degrees >= 0)
            {
                muxer.SetOrientationHint(degrees);
            }
        }
        if (startMs > 0)
        {
            extractor.SeekTo(startMs * 1000, MediaExtractorSeekTo.ClosestSync);
        }
        // Copy the samples from MediaExtractor to MediaMuxer. We will loop
        // for copying each sample and stop when we get to the end of the source
        // file or exceed the end time of the trimming.
        int offset = 0;
        int trackIndex = -1;
        ByteBuffer dstBuf = ByteBuffer.Allocate(bufferSize);
        MediaCodec.BufferInfo bufferInfo = new MediaCodec.BufferInfo();
        try
        {
            muxer.Start();
            while (true)
            {
                bufferInfo.Offset = offset;
                bufferInfo.Size = extractor.ReadSampleData(dstBuf, offset);
                if (bufferInfo.Size < 0)
                {
                    bufferInfo.Size = 0;
                    break;
                }
                else
                {
                    bufferInfo.PresentationTimeUs = extractor.SampleTime;
                    if (endMs > 0 && bufferInfo.PresentationTimeUs > endMs * 1000)
                    {
                        //Log.d(LOGTAG, "The current sample is over the trim end time.");
                        break;
                    }
                    else
                    {
                        int flagValue = (int)extractor.SampleFlags;
                        bufferInfo.Flags = (MediaCodecBufferFlags)flagValue;
                        trackIndex = extractor.SampleTrackIndex;
                        muxer.WriteSampleData(indexMap[trackIndex], dstBuf, bufferInfo);
                        extractor.Advance();
                    }
                }
            }
            muxer.Stop();
        }
        catch (IllegalStateException e)
        {
            // Swallow the exception due to malformed source.
            //Log.w(LOGTAG, "The source video file is malformed");
        }
        finally
        {
            muxer.Release();
        }
        return;
    }

    private void SplitVideoWithMp4Parser(byte[] video, string outputPath, int startMs, int endMs)
    {
        IDataSource source = new MemoryDataSourceImpl(video);
        var movie = MovieCreator.Build(source);
        var tracks = movie.Tracks;

        movie.Tracks = new List<ITrack>();
        // remove all tracks we will create new tracks from the old

        double startTime1 = startMs;
        double endTime1 = endMs;
        double startTime2 = startMs;
        double endTime2 = endMs;

        bool timeCorrected = false;

        // Here we try to find a track that has sync samples. Since we can only start decoding
        // at such a sample we SHOULD make sure that the start of the new fragment is exactly
        // such a frame
        foreach (var track in tracks)
        {
            if (track.GetSyncSamples() != null && track.GetSyncSamples().Length > 0)
            {
                if (timeCorrected)
                {
                    // This exception here could be a false positive in case we have multiple tracks
                    // with sync samples at exactly the same positions. E.g. a single movie containing
                    // multiple qualities of the same video (Microsoft Smooth Streaming file)

                    throw new RuntimeException("The startTime has already been corrected by another track with SyncSample. Not Supported.");
                }
                startTime1 = CorrectTimeToSyncSample(track, startTime1, false);
                endTime1 = CorrectTimeToSyncSample(track, endTime1, true);
                startTime2 = CorrectTimeToSyncSample(track, startTime2, false);
                endTime2 = CorrectTimeToSyncSample(track, endTime2, true);
                timeCorrected = true;
            }
        }

        foreach (var track in tracks)
        {
            long currentSample = 0;
            double currentTime = 0;
            double lastTime = -1;
            long startSample1 = -1;
            long endSample1 = -1;
            long startSample2 = -1;
            long endSample2 = -1;

            for (int i = 0; i < track.GetSampleDurations().Length; i++)
            {
                long delta = track.GetSampleDurations()[i];


                if (currentTime > lastTime && currentTime <= startTime1)
                {
                    // current sample is still before the new starttime
                    startSample1 = currentSample;
                }
                if (currentTime > lastTime && currentTime <= endTime1)
                {
                    // current sample is after the new start time and still before the new endtime
                    endSample1 = currentSample;
                }
                if (currentTime > lastTime && currentTime <= startTime2)
                {
                    // current sample is still before the new starttime
                    startSample2 = currentSample;
                }
                if (currentTime > lastTime && currentTime <= endTime2)
                {
                    // current sample is after the new start time and still before the new endtime
                    endSample2 = currentSample;
                }
                lastTime = currentTime;
                currentTime += (double)delta / track.TrackMetaData.Timescale;
                currentSample++;
            }
            movie.AddTrack(new AppendTrack(new CroppedTrack(track, startSample1, endSample1), new CroppedTrack(track, startSample2, endSample2)));
        }

        BasicContainer outContainer = (BasicContainer)new DefaultMp4Builder().Build(movie);
        FileOutputStream fos = new FileOutputStream(outputPath);
        FileChannel fc = fos.Channel;
        outContainer.WriteContainer(fc);
        fc.Close();
        fos.Close();


        double CorrectTimeToSyncSample(ITrack track, double cutHere, bool next)
        {
            double[] timeOfSyncSamples = new double[track.GetSyncSamples().Length];
            long currentSample = 0;
            double currentTime = 0;
            for (int i = 0; i < track.GetSampleDurations().Length; i++)
            {
                long delta = track.GetSampleDurations()[i];

                if (Array.BinarySearch(track.GetSyncSamples(), currentSample + 1) >= 0)
                {
                    // samples always start with 1 but we start with zero therefore +1
                    timeOfSyncSamples[Array.BinarySearch(track.GetSyncSamples(), currentSample + 1)] = currentTime;
                }
                currentTime += delta / (double)track.TrackMetaData.Timescale;
                currentSample++;

            }
            double previous = 0;
            foreach (double timeOfSyncSample in timeOfSyncSamples)
            {
                if (timeOfSyncSample > cutHere)
                {
                    if (next)
                    {
                        return timeOfSyncSample;
                    }
                    else
                    {
                        return previous;
                    }
                }
                previous = timeOfSyncSample;
            }
            return timeOfSyncSamples[timeOfSyncSamples.Length - 1];
        }
    }
}

public enum MediaType
{
    All,
    Image,
    Video
}