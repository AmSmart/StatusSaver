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
    public partial class ImageViewerPage : ContentPage
    {
        public ImageViewerPage()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<ImageViewerViewModel>();
        }

        public ImageViewerPage(int index, IEnumerable<string> paths)
        {
            InitializeComponent();
            var bc = Startup.ServiceProvider.GetService<ImageViewerViewModel>();
            bc.CurrentPosition = index;
            bc.Paths = paths;
            BindingContext = bc;
        }
    }
}