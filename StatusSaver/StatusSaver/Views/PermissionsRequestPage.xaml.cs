﻿using StatusSaver.ViewModels;
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
    public partial class PermissionsRequestPage : ContentPage
    {
        public PermissionsRequestPage()
        {
            InitializeComponent();
            BindingContext = Startup.ServiceProvider.GetService<PermissionsRequestViewModel>();
        }

        protected override void OnAppearing()
        {
            var bc = (PermissionsRequestViewModel)BindingContext;
            
            if (bc.CheckAccessGranted())
            {
                bc.NavigateHome();
            }

            base.OnAppearing();
        }
    }
}