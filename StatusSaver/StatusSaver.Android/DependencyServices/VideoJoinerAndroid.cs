using Com.Googlecode.Mp4parser;
using Com.Googlecode.Mp4parser.Authoring;
using Com.Googlecode.Mp4parser.Authoring.Builder;
using Com.Googlecode.Mp4parser.Authoring.Container.Mp4;
using Com.Googlecode.Mp4parser.Authoring.Tracks;
using Java.IO;
using StatusSaver.DependencyServices;
using StatusSaver.Droid.DependencyServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(VideoJoinerAndroid))]
namespace StatusSaver.Droid.DependencyServices
{
    public class VideoJoinerAndroid : IVideoJoiner
    {
        public void MergeVideos(string[] pathNames, string outputPath)
        {
            List<Movie> inMovies = new List<Movie>();
            Movie movie = new Movie();

            foreach(string path in pathNames )
            {
                inMovies.Add(MovieCreator.Build(path));
            }

            List<List<ITrack>> allTracks = new List<List<ITrack>>();
            for (int i = 0; i < pathNames.Length; i++)
            {
                allTracks.Add(new List<ITrack>());
            }

            for (int i = 0; i < inMovies.Count(); i++)
            {
                foreach(ITrack track in inMovies[i].Tracks)                
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
            FileOutputStream fos = new FileOutputStream(new File(outputPath));
            oout.WriteContainer(fos.Channel);
            fos.Close();
        }
    }
}
