using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StausSaver.Maui
{
    public static class Settings
    {
        public static string MediaFolderUri 
        { 
            get => Preferences.Get(nameof(MediaFolderUri), null); 
            set => Preferences.Set(nameof(MediaFolderUri), value); 
        }
    }
}
