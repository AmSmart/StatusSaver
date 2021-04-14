using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StatusSaver.ServicesAbstract
{
    public interface IPageManager
    {
        Task DisplayAlert(string title, string message, string cancel);

        Task<string> DisplayPrompt(string title, string message, string accept);

        Task NavigateTo(string route);
        
        Task<bool> ExitPrompt();

        void PopPage();

        void UpdateSelectionState(bool selectionModeIsMultiple, IList<ToolbarItem> toolbarItems);
    }
}
