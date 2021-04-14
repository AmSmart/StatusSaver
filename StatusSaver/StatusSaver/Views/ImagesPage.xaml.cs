using StatusSaver.DependencyServices;
using StatusSaver.Services.Abstract;
using StatusSaver.ServicesConcrete;
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
    public partial class ImagesPage : ContentPage
    {
        public ImagesPage()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<ImagesPageViewModel>();
            Startup.ServiceProvider.GetService<VideosPageViewModel>();
        }

        protected override bool OnBackButtonPressed()
        {
            var bc = BindingContext as ImagesPageViewModel;
            bc.OnBackButtonPressed();
            return true;
        }
    }
}