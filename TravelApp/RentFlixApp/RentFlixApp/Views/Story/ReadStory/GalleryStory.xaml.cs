using System.Collections.Generic;
using System.Threading.Tasks;
using RentFlixApp.Helpers;
using RentFlixApp.Models;
using RentFlixApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentFlixApp.Views.Story.ReadStory
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GalleryStory : ContentPage
    {
        #region private properties
        private readonly StoryServices _storyServices = new StoryServices();
        private int _postId;


        public int PostId
        {
            get => _postId;
            set { _postId = value; OnPropertyChanged(); }
        }
        #endregion
        public GalleryStory()
        {
            BindingContext = this;

            InitializeComponent();
        }


        protected override async void OnAppearing()
        {
            await GetImages();
            base.OnAppearing();
        }

        private async Task GetImages()
        {
            IsBusy = true;
            var galleryList = new List<Gallery>();
            var photos = await _storyServices.GetStoryPhotoGallery(Settings.AccessToken, PostId);
            foreach (var photo in photos)
            {
                galleryList.Add(new Gallery()
                {
                    Description = photo.Description,
                    FullPath = Constants.BaseUrlNoSlash + photo.FullPath
                });
            }
            MainCarouselView.ItemsSource = galleryList;
            IsBusy = false;
        }

        private void PictureTouched(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Gallery text)
                ImageDescription.Text = text.Description;
        }
    }
}