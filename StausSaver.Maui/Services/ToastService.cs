using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace StatusSaver.Maui.Services;

public class ToastService
{
    public async Task ShowShortToast(string message, int fontSize = 14, CancellationToken cancellationToken = default)
    {
        ToastDuration duration = ToastDuration.Short;
        var toast = Toast.Make(message, duration, fontSize);
        await toast.Show(cancellationToken);
    }
    
    public async Task ShowLongToast(string message, int fontSize = 14, CancellationToken cancellationToken = default)
    {
        ToastDuration duration = ToastDuration.Long;
        var toast = Toast.Make(message, duration, fontSize);
        await toast.Show(cancellationToken);
    }
}
