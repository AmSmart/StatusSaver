using StatusSaver.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StatusSaver.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MediaPlayerPage : ContentPage
    {
        public MediaPlayerPage()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<MediaPlayerViewModel>();
        }

        public MediaPlayerPage(int index, IEnumerable<string> paths)
        {
            InitializeComponent();
            var bc = Startup.ServiceProvider.GetService<MediaPlayerViewModel>();
            bc.CurrentPosition = index;
            bc.Paths = paths;
            BindingContext = bc;
        }        
    }
}