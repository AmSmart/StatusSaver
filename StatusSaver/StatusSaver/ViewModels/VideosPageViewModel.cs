using MvvmHelpers;
using Newtonsoft.Json;
using StatusSaver.DependencyServices;
using StatusSaver.Models;
using StatusSaver.Services.Abstract;
using StatusSaver.ServicesAbstract;
using StatusSaver.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace StatusSaver.ViewModels
{
    public class VideosPageViewModel : BaseViewModel
    {
        private SelectionMode _selectionMode;
        private Color _backgroundColor;

        private readonly IPageManager _pageManager;
        private readonly IThumbnailGenerator _thumbnailGenerator;
        private readonly IMediaManager _mediaManager;
        private readonly IMessenger _messenger;
        private bool _isRefreshing;
        private readonly Color _selectedStateColor = Color.DodgerBlue;
        private readonly Color _unselectedStateColor = Color.White;
        private readonly IEnumerable<string> _statusResourcesPaths;
        private readonly string _appCachePath;
        private readonly IList<ToolbarItem> _toolbarItems;


        public VideosPageViewModel(IPathManager pathManager, IPageManager pageManager,
            IThumbnailGenerator thumbnailGenerator, IMediaManager mediaManager, IMessenger message)
        {
            BackgroundColor = Color.White;
            SelectionMode = SelectionMode.None;
            SingleTap = new Command(OnSingleTap);
            LongPressed = new Command(OnLongPressed);
            Save = new Command(OnSave);
            SaveAs = new Command(OnSaveAs);
            Cancel = new Command(OnCancel);
            JoinAndSave = new Command(OnJoinAndSave);
            RefreshList = new Command(OnRefresh);

            Videos = new ObservableCollection<Video>();
            SelectedItems = new ObservableCollection<object>();

            _toolbarItems = new List<ToolbarItem>()
            {
                new ToolbarItem()
                {
                    Text = "Save",
                    Command = Save
                },
                new ToolbarItem()
                {
                    Text = "Save As",
                    Command = SaveAs
                },
                new ToolbarItem()
                {
                    Text = "Join and Save",
                    Command = JoinAndSave
                },
                new ToolbarItem()
                {
                    Text = "Cancel",
                    Command = Cancel
                }
            };

            _statusResourcesPaths = pathManager.GetStatusResourcesPaths();
            _appCachePath = pathManager.GetAppCachePath();

            Task.Run(() =>
            {
                LoadData(thumbnailGenerator);
            });

            _pageManager = pageManager;
            _thumbnailGenerator = thumbnailGenerator;
            _mediaManager = mediaManager;
            _messenger = message;
        }

        private void OnRefresh(object obj)
        {
            Task.Run(() =>
            {
                _mediaManager.ClearAppCache();
                LoadData(_thumbnailGenerator);
                IsRefreshing = false;
            });
        }

        private void LoadData(IThumbnailGenerator thumbnailGenerator)
        {
            List<string> allVideoUrls = new List<string>();
            foreach (var path in _statusResourcesPaths)
            {
                if (Directory.Exists(path))
                {
                    var files = Directory.GetFiles(path).Where(x => x.EndsWith(".mp4"));
                    allVideoUrls.AddRange(files);
                }
            }
             
            Videos.Clear();
            foreach (string url in allVideoUrls)
            {
                Videos.Add(new Video
                {
                    ImageCachePath = thumbnailGenerator.GenerateThumbnailAsPath(url, 1000, _appCachePath),
                    ImageSource = null,/*thumbnailGenerator.GenerateThumbnail(url, 1000),*/
                    Path = url,
                    BackgroundColor = _unselectedStateColor
                });
            }
        }

        public ObservableCollection<Video> Videos { get; set; }

        public SelectionMode SelectionMode
        {
            get => _selectionMode;
            set => SetProperty(ref _selectionMode, value, onChanged: () =>
            {
                if(value == SelectionMode.Multiple)
                {
                    _pageManager.UpdateSelectionState(true, _toolbarItems);
                }
                else
                {
                    _pageManager.UpdateSelectionState(false, null);
                }
            });
        }

        public Color BackgroundColor
        {
            get => _backgroundColor;
            set => SetProperty(ref _backgroundColor, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public ICommand RefreshList { get; set; }

        public ICommand SingleTap { get; set; }

        public ICommand LongPressed { get; set; }

        public ICommand Save { get; set; }

        public ICommand SaveAs { get; set; }

        public ICommand JoinAndSave { get; set; }

        public ICommand Cancel { get; set; }

        public ObservableCollection<object> SelectedItems { get; set; }

        private void OnSingleTap(object obj)
        {
            if (SelectionMode == SelectionMode.Multiple)
            {
                if (!SelectedItems.Contains(obj))
                {
                    SelectedItems.Add(obj);
                    Videos.First(x => x == obj as Video).BackgroundColor = _selectedStateColor;
                }
                else
                {
                    SelectedItems.Remove(obj);
                    Videos.First(x => x == obj as Video).BackgroundColor = _unselectedStateColor;

                    if (SelectedItems.Count == 0)
                        SelectionMode = SelectionMode.None;
                }
            }
            else
            {
                //var video = obj as Video;
                //string path = video.Path;
                //_pageManager.NavigateTo($"//home/videos/mediaplayer?videoPath={path}");

                int index = Videos.IndexOf(obj as Video);
                var paths = Videos.Select(x => x.Path);
                Application.Current.MainPage.Navigation.PushAsync(new MediaPlayerPage(index,paths));
            }
        }

        private void OnLongPressed(object obj)
        {
            SelectionMode = SelectionMode.Multiple;

            if (SelectedItems.Contains(obj))
            {
                SelectedItems.Remove(obj);
                Videos.First(x => x == obj as Video).BackgroundColor = _unselectedStateColor;

                if (SelectedItems.Count == 0)
                    SelectionMode = SelectionMode.None;
            }
            else
            {
                SelectedItems.Add(obj);
                Videos.First(x => x == obj as Video).BackgroundColor = _selectedStateColor;
            }
        }

        private void OnSave()
        {
            if (SelectedItems.Count < 1)
            {
                _pageManager.DisplayAlert("Error!", "No Items selected", "Got it!");
            }
            else if (SelectedItems.Count == 1)
            {
                _mediaManager.SaveSingle((SelectedItems[0] as Video).Path, FileType.Video, name: "video");
                ClearSelection();
                _pageManager.DisplayAlert("Done!", "File Saved Successfully", "Got it!");
            }
            else
            {
                _mediaManager.SaveMultiple(SelectedItems.Cast<Video>().Select(x => x.Path).ToArray(),
                    FileType.Video, "video");
                ClearSelection();
                _pageManager.DisplayAlert("Done!", "Files Saved Successfully", "Got it!");
            }
        }

        private async void OnSaveAs()
        {
            if (SelectedItems.Count < 1)
            {
                await _pageManager.DisplayAlert("Error!", "No Items selected", "Got it!");
                return;
            }

            string result = await _pageManager.DisplayPrompt("Save File?", "Enter file name", "Save");

            if (result == null)
                return;

            // string validation required

            else if (SelectedItems == null || SelectedItems.Count < 1)
            {
                _mediaManager.SaveSingle((SelectedItems[0] as Video).Path, FileType.Video, result);
                SelectedItems.Clear();
                SelectionMode = SelectionMode.None;
                await _pageManager.DisplayAlert("Done!", "File Saved Successfully", "Got it!");
            }
            else
            {
                _mediaManager.SaveMultiple(SelectedItems.Cast<Video>().Select(x => x.Path).ToArray(),
                    FileType.Video, result);
                ClearSelection();
                await _pageManager.DisplayAlert("Done!", "Files Saved Successfully", "Got it!");
            }
        }

        private async void OnJoinAndSave()
        {
            if (SelectedItems == null || SelectedItems.Count < 1)
            {
                await _pageManager.DisplayAlert("Error!", "No Items selected", "Got it!");
                return;
            }

            else if (SelectedItems.Count == 1)
            {
                _messenger.LongAlert("Only one video is selected");
            }

            string result = await _pageManager.DisplayPrompt("Save File?", "Enter file name", "Save");

            if (result == null)
                return;

            // string validation required

            else
            {
                try
                {
                    _mediaManager.SaveVideosAsOne(SelectedItems.Cast<Video>().Select(x => x.Path).ToArray(),
                   result);
                    ClearSelection();
                    await _pageManager.DisplayAlert("Done!", "Files Saved Successfully", "Got it!");
                }
                catch
                {
                    ClearSelection();
                    _messenger.ShortAlert("You cannot join videos with different dimensions");
                }
            }
        }

        private void OnCancel()
        {
            ClearSelection();
            _messenger.LongAlert("All selections cancelled");
        }

        private void ClearSelection()
        {
            SelectedItems.Clear();
            SelectionMode = SelectionMode.None;

            foreach (var video in Videos)
            {
                video.BackgroundColor = _unselectedStateColor;
            }
        }

        public async void OnBackButtonPressed()
        {
            if (SelectionMode == SelectionMode.Multiple)
            {
                OnCancel();
            }
            else
            {
                bool exit = await _pageManager.ExitPrompt();
                if (exit)
                {
                    _pageManager.PopPage();
                }
            }
        }
    }
}
