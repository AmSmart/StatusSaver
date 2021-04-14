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
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<AboutPageViewModel>();
        }

        protected override bool OnBackButtonPressed()
        {
            var bc = BindingContext as AboutPageViewModel;
            bc.OnBackButtonPressed();
            return true;
        }
    }
}