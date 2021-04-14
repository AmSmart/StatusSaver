using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusSaver.Models
{
    public class VideoPath
    {
        public VideoPath(string path)
        {
            Path = path;
        }

        public string Path { get; set; }
    }
}
