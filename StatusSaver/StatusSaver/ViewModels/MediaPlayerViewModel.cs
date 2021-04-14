using MvvmHelpers;
using StatusSaver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace StatusSaver.ViewModels
{
    public class MediaPlayerViewModel : BaseViewModel
    {
        private int _currentPosition;
        private IEnumerable<string> _paths;

        public IEnumerable<string> Paths
        {
            get => _paths;
            set => SetProperty(ref _paths, value);
        }

        public int CurrentPosition
        {
            get => _currentPosition;
            set => SetProperty(ref _currentPosition, value);
        }
    }
}
