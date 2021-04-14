using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusSaver.DependencyServices
{
    public interface IVideoJoiner
    {
        void MergeVideos(string[] pathNames, string outputPath);
    }
}
