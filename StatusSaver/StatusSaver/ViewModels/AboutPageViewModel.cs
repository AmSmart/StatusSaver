using MvvmHelpers;
using StatusSaver.ServicesAbstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusSaver.ViewModels
{
    public class AboutPageViewModel : BaseViewModel
    {
        private readonly IPageManager _pageManager;

        public AboutPageViewModel(IPageManager pageManager)
        {
            _pageManager = pageManager;
        }

        public async void OnBackButtonPressed()
        {
            bool exit = await _pageManager.ExitPrompt();
            if (exit)
            {
                _pageManager.PopPage();
            }
        }
    }
}
