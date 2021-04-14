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
    public partial class VideosPage : ContentPage
    {
        public VideosPage()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<VideosPageViewModel>();
        }

        protected override bool OnBackButtonPressed()
        {
            var bc = BindingContext as VideosPageViewModel;
            bc.OnBackButtonPressed();
            return true;
        }

    }
}