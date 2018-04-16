using System;
using Plugin.Media;
using Plugin.Media.Abstractions;
using RentFlixApp.Helpers;
using RentFlixApp.Models;
using RentFlixApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentFlixApp.Views.Story.CreateStory
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadGallery : ContentPage
    {
        #region private properties
        private string _descriptionImage;
        private readonly int _storyId;
        private MediaFile _mediaFile;
        private readonly StoryServices _storyServices = new StoryServices();
        public string ImageDescription
        {
            get => _descriptionImage;
            set
            {
                _descriptionImage = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public UploadGallery(int storyId)
        {
            BindingContext = this;
            _storyId = storyId;
            InitializeComponent();
        }

        private async void UploadPhoto(object sender, EventArgs e)
        {
           //upload to database
            IsBusy = true;
            var imagePath = await _storyServices.UploadPhoto(Settings.AccessToken, _mediaFile);
            var gallery = new Gallery()
            {
                Description = ImageDescription,
                FullPath = imagePath.Replace("\"",""), //remove quotes from link
                StoriesModelId = _storyId
            };
            var isSuccess = await _storyServices.AddPhotoToGallery(Settings.AccessToken, gallery);

            if (isSuccess)
                await DisplayAlert("Status", "Image successfully added to story", "Ok");

            //clean variables for others to come
            IsBusy = false;
            ImageDescription = "";
            GalleryImage.Source = "uploadImage.png";
        }

 
        private async void ViewGallery(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void PickPhoto(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("No pickfoto", "No pickfoto available", "ok");
                return;
            }

            _mediaFile = await CrossMedia.Current.PickPhotoAsync();
            if (_mediaFile == null) return;
            GalleryImage.Source = ImageSource.FromStream(() => _mediaFile.GetStream());
        }
    }
}