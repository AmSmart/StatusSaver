using StausSaver.Maui.ViewModels;

namespace StatusSaver.Maui.ViewModels;

public partial class ImageViewerViewModel : ViewModelBase, IQueryAttributable
{
    [ObservableProperty]
    private ObservableCollection<string> _imageUris;
    
    [ObservableProperty]
    private string _currentImageUri;

    private int _currentIndex;

    [RelayCommand]
    void SwipeLeft()
    {
        if (_currentIndex == ImageUris.Count - 1)
        {
            _currentIndex = 0;
        }
        else
        {
            _currentIndex++;
        }
        CurrentImageUri = ImageUris[_currentIndex];
    }

    [RelayCommand]
    void SwipeRight()
    {
        if (_currentIndex == 0)
        {
            _currentIndex = ImageUris.Count - 1;
        }
        else
        {
            _currentIndex--;
        }
        CurrentImageUri = ImageUris[_currentIndex];
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        ImageUris = (ObservableCollection<string>) query[nameof(ImageUris)];
        CurrentImageUri = (string) query[nameof(CurrentImageUri)];
        _currentIndex = ImageUris.IndexOf(CurrentImageUri);
    }
}