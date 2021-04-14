using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StatusSaver.ViewModels
{
    public class PermissionsDeniedViewModel : BaseViewModel
    {
        public PermissionsDeniedViewModel()
        {
            LaunchSettings = new Command(() => AppInfo.ShowSettingsUI());
        }

        public ICommand LaunchSettings { get; set; }        
    }
}
