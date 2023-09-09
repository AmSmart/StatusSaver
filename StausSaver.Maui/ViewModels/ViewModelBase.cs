using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StausSaver.Maui.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    string title;

    [ObservableProperty]
    string subtitle;

    [ObservableProperty]
    string icon;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;

    public bool IsNotBusy => !isBusy;

    [ObservableProperty]
    bool canLoadMore;

    [ObservableProperty]
    string header;

    [ObservableProperty]
    string footer;
}
