using StatusSaver.ServicesAbstract;
using StatusSaver.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StatusSaver.ServicesConcrete
{
    public class PageManager : IPageManager
    {
        public Page CurrentPage { get; set; } = Application.Current.MainPage;

        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await CurrentPage.DisplayAlert(title, message, cancel);
        }

        public async Task<string> DisplayPrompt(string title, string message, string accept)
        {
            return await CurrentPage.DisplayPromptAsync(title, message, accept: accept);
        }

        public async Task NavigateTo(string route)
        {
            await (CurrentPage as Shell).GoToAsync(route, true);
        }

        public async Task<bool> ExitPrompt()
        {
            string title = "Done?";
            string question = "Are you sure you want to leave?";
            string yes = "Yes";
            string no = "No";

            return await CurrentPage.DisplayAlert(title, question, yes, no);
        }

        public void PopPage()
        {
            Environment.Exit(0);
        }

        public void UpdateSelectionState(bool selectionModeIsMultiple, IList<ToolbarItem> toolbarItems)
        {
            var color = selectionModeIsMultiple ? Color.Gray : Color.FromHex("2196F3");
            CurrentPage.SetValue(Shell.BackgroundColorProperty, color);
            UpdateToolbarItems(toolbarItems);
        }

        /// <summary>
        /// Updates toolbar items in a Shell's currently presented page
        /// Call with caution, when the shell hierarchy is invalid, can generate null reference exception or update different page
        /// </summary>
        /// <param name="toolbarItems"></param>
        private void UpdateToolbarItems(IList<ToolbarItem> toolbarItems)
        {
            // Get the inner page of a ShellItem/FlyoutItem
            var page = ((CurrentPage as Shell)?.CurrentItem?.CurrentItem as IShellSectionController)?.PresentedPage;

            page.ToolbarItems.Clear();

            if (toolbarItems != null)
            {
                foreach (var toolbarItem in toolbarItems)
                {
                    page.ToolbarItems.Add(toolbarItem);
                }
            }
        }
    }
}
