using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StatusSaver.Models
{
    public class Video : ObservableObject
    {
        private ImageSource _imageSource;
        private string _path;
        private Color _backgroundColor = Color.White;
        private string _imageCachePath;

        public ImageSource ImageSource { get => _imageSource; set => SetProperty(ref _imageSource, value); }

        public string Path { get => _path; set => SetProperty(ref _path, value); }

        public string ImageCachePath { get => _imageCachePath; set => SetProperty(ref _imageCachePath, value); }

        public Color BackgroundColor { get => _backgroundColor; set => SetProperty(ref _backgroundColor, value); }
    }
}
