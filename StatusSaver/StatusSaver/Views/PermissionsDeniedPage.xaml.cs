using StatusSaver.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StatusSaver.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PermissionsDeniedPage : ContentPage
    {
        public PermissionsDeniedPage()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<PermissionsDeniedViewModel>();
        }
    }
}