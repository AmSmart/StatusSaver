using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StatusSaver.Models
{
    public class Image : ObservableObject
    {
        private string _path;
        private Color _backgroundColor = Color.White;

        public string Path { get => _path; set => SetProperty(ref _path, value); }

        public Color BackgroundColor { get => _backgroundColor; set => SetProperty(ref _backgroundColor, value); }
    }
}
