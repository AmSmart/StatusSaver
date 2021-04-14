using StatusSaver.Views;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace StatusSaver
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("imageviewer", typeof(ImageViewerPage));
            Routing.RegisterRoute("mediaplayer", typeof(MediaPlayerPage));
        }
    }
}