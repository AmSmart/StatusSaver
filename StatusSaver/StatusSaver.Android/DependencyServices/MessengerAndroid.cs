using Android.App;
using Android.Widget;
using StatusSaver.DependencyServices;
using StatusSaver.Droid.DependencyServices;

[assembly: Xamarin.Forms.Dependency(typeof(MessengerAndroid))]
namespace StatusSaver.Droid.DependencyServices
{
    public class MessengerAndroid : IMessenger
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}