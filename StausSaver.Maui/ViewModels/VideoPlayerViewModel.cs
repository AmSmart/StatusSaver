using CommunityToolkit.Maui.Views;
using StausSaver.Maui.ViewModels;

namespace StatusSaver.Maui.ViewModels;

public partial class VideoPlayerViewModel : ViewModelBase, IQueryAttributable
{
    [ObservableProperty]
    private ObservableCollection<string> _videoUris;

    [ObservableProperty]
    private string _currentVideoUri;

    private int _currentIndex;


    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        VideoUris = (ObservableCollection<string>)query[nameof(VideoUris)];
        string currentVideoUri = (string) query["CurrentVideoUri"];
        _currentIndex = VideoUris.IndexOf(currentVideoUri);
        CurrentVideoUri = currentVideoUri;
    }

    [RelayCommand]
    void SwipeLeft()
    {
        if (_currentIndex == VideoUris.Count - 1)
        {
            _currentIndex = 0;
        }
        else
        {
            _currentIndex++;
        }
        CurrentVideoUri = VideoUris[_currentIndex];
    }

    [RelayCommand]
    void SwipeRight()
    {
        if (_currentIndex == 0)
        {
            _currentIndex = VideoUris.Count - 1;
        }
        else
        {
            _currentIndex--;
        }
        CurrentVideoUri = VideoUris[_currentIndex];
    }
}
