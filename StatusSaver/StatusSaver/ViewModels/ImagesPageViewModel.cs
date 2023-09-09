using MvvmHelpers;
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
using Image = StatusSaver.Models.Image;

namespace StatusSaver.ViewModels
{
    public class ImagesPageViewModel : BaseViewModel
    {
        private Color _backgroundColor;
        private bool _isRefreshing;

        private readonly IPageManager _pageManager;
        private readonly IMediaManager _mediaManager;
        private readonly IMessenger _messenger;
        private SelectionMode _selectionMode;
        private readonly Color _selectedStateColor = Color.DodgerBlue;
        private readonly Color _unselectedStateColor = Color.White;
        private readonly IList<ToolbarItem> _toolbarItems;

        public ImagesPageViewModel(IPageManager pageManager, IMediaManager mediaSaver,
            IMessenger message)
        {
            BackgroundColor = Color.White;
            SelectionMode = SelectionMode.None;
            SingleTap = new Command(OnSingleTap);
            LongPressed = new Command(OnLongPressed);
            Save = new Command(OnSave);
            SaveAs = new Command(OnSaveAs);
            Cancel = new Command(OnCancel);
            RefreshList = new Command(OnRefresh);

            Images = new ObservableCollection<Image>();
            SelectedItems = new ObservableCollection<object> { };

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
                    Text = "Cancel",
                    Command = Cancel
                }
            };

            _pageManager = pageManager;
            _mediaManager = mediaSaver;
            _messenger = message;

            Task.Run(() =>
            {
                _mediaManager.LoadImages(Images);
            });
        }

        private void OnRefresh(object obj)
        {
            Task.Run(() =>
            {
                _mediaManager.LoadImages(Images);
                IsRefreshing = false;
            });
        }

        public ObservableCollection<Image> Images { get; set; }

        public Color BackgroundColor
        {
            get => _backgroundColor;
            set => SetProperty(ref _backgroundColor, value);
        }

        public SelectionMode SelectionMode
        {
            get => _selectionMode;
            set => SetProperty(ref _selectionMode, value, onChanged: () =>
            {
                if (value == SelectionMode.Multiple)
                {
                    _pageManager.UpdateSelectionState(true, _toolbarItems);
                }
                else
                {
                    _pageManager.UpdateSelectionState(false, null);
                }
            });
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public bool ReadyToClose { get; set; }

        public ICommand RefreshList { get; set; }

        public ICommand SingleTap { get; set; }

        public ICommand LongPressed { get; set; }

        public ICommand Save { get; set; }

        public ICommand SaveAs { get; set; }

        public ICommand Cancel { get; set; }

        public ObservableCollection<object> SelectedItems { get; set; }

        private void OnSingleTap(object obj)
        {
            if (SelectionMode == SelectionMode.Multiple)
            {
                if (!SelectedItems.Contains(obj))
                {
                    SelectedItems.Add(obj);
                    Images.First(x => x == obj as Image).BackgroundColor = _selectedStateColor;
                }
                else
                {
                    SelectedItems.Remove(obj);
                    Images.First(x => x == obj as Image).BackgroundColor = _unselectedStateColor;

                    if (SelectedItems.Count == 0)
                        SelectionMode = SelectionMode.None;
                }
            }
            else
            {
                //string path = (obj as Image).Path;
                //_pageManager.NavigateTo($"//home/images/imageviewer?imagePath={path}");

                int index = Images.IndexOf(obj as Image);
                var paths = Images.Select(x => x.Path);
                Application.Current.MainPage.Navigation.PushAsync(new ImageViewerPage(index, paths));
            }
        }

        private void OnLongPressed(object obj)
        {
            SelectionMode = SelectionMode.Multiple;

            if (SelectedItems.Contains(obj))
            {
                SelectedItems.Remove(obj);
                Images.First(x => x == obj as Image).BackgroundColor = _unselectedStateColor;

                if (SelectedItems.Count == 0)
                    SelectionMode = SelectionMode.None;
            }
            else
            {
                SelectedItems.Add(obj);
                Images.First(x => x == obj as Image).BackgroundColor = _selectedStateColor;
            }
        }

        private void OnSave()
        {
            if (SelectedItems == null || SelectedItems.Count < 1)
            {
                _pageManager.DisplayAlert("Error!", "No Items selected", "Got it!");
            }
            else if (SelectedItems.Count == 1)
            {
                _mediaManager.SaveSingle((SelectedItems[0] as Image).Path, FileType.Image);
                ClearSelection();
                _pageManager.DisplayAlert("Done!", "File Saved Successfully", "Got it!");
            }
            else
            {
                _mediaManager.SaveMultiple(SelectedItems.Cast<Image>().Select(x => x.Path).ToArray(),
                    FileType.Image);
                ClearSelection();
                _pageManager.DisplayAlert("Done!", "Files Saved Successfully", "Got it!");
            }
        }

        private async void OnSaveAs()
        {
            if (SelectedItems == null || SelectedItems.Count < 1)
            {
                await _pageManager.DisplayAlert("Error!", "No Items selected", "Got it!");
                return;
            }

            string result = await _pageManager.DisplayPrompt("Save File?", "Enter file name", "Save");

            if (result == null)
                return;

            // string validation required

            else if (SelectedItems.Count == 1)
            {
                _mediaManager.SaveSingle((SelectedItems[0] as Image).Path, FileType.Image, result);
                ClearSelection();
                await _pageManager.DisplayAlert("Done!", "File Saved Successfully", "Got it!");
            }
            else
            {
                _mediaManager.SaveMultiple(SelectedItems.Cast<Image>().Select(x => x.Path).ToArray(),
                    FileType.Image, result);
                ClearSelection();
                await _pageManager.DisplayAlert("Done!", "Files Saved Successfully", "Got it!");
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

            foreach (var image in Images)
            {
                image.BackgroundColor = _unselectedStateColor;
            }
        }

        public void OnBackButtonPressed()
        {
            if (SelectionMode == SelectionMode.Multiple)
            {
                OnCancel();
            }
            else
            {
                ReadyToClose = true;
                _messenger.ShortAlert("Go back again to exit");
            }
        }
    }
}
