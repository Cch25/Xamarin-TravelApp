using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RentFlixApp.Helpers;
using RentFlixApp.Models;
using RentFlixApp.Pages;
using RentFlixApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentFlixApp.Views.Story.CreateStory
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateGalleryStory : ContentPage
    {
        #region private properties
        private readonly int _storyId;
        private readonly StoryServices _storyServices = new StoryServices();
        private IList<Gallery> _galleryList;

        public IList<Gallery> GalleryList
        {
            get => _galleryList;
            set
            {
                _galleryList = value;
                OnPropertyChanged();
            }
        }

        #endregion
        public CreateGalleryStory(int id)
        {
            _storyId = id;
            BindingContext = this;
            InitializeComponent();
        }

        private void UploadPhotos(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new UploadGallery(_storyId));
        }

        public async void GenerateGallery()
        {
            IsBusy = true;
            var gallery = await _storyServices.GetStoryPhotoGallery(Settings.AccessToken, _storyId);
            GalleryList = new ObservableCollection<Gallery>();
            foreach (var g in gallery)
            {
                GalleryList.Add(
                    new Gallery
                    {
                        Description = g.Description,
                        FullPath = Constants.BaseUrlNoSlash + g.FullPath,
                    });
            }
            InfoLabel.Text = GalleryList.Count <= 0 ? "No photos yet." : "";
            IsBusy = false;
        }

        private void RefreshList(object sender, EventArgs e)
        {
            GenerateGallery();
            ListView.EndRefresh();
        }

        private async void ItemTap(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as Gallery;
            await Navigation.PushModalAsync(new ViewFullPhoto(item));
        }

        private async void DoneWithStory(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new MasterMenu());
        }
    }
}