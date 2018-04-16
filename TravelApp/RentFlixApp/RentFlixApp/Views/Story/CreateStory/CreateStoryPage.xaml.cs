using System;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using RentFlixApp.Extensions;
using RentFlixApp.Helpers;
using RentFlixApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentFlixApp.Views.Story.CreateStory
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateStoryPage : ContentPage
    {
        #region Properties
        private MediaFile _mediaFile;
        private UserProfileService _ups = new UserProfileService();
        private readonly StoryServices _storyServices = new StoryServices();
        private string _headTitle;
        private string _region;
        private string _location;
        private string _myStory;
        private int _rating;
        private string _coverImage;

        public string CoverImage
        {
            get => _coverImage;
            set { _coverImage = value; OnPropertyChanged(); }
        }

        public int Rating
        {
            get => _rating;
            set { _rating = value; OnPropertyChanged(); }
        }

        public string StoryText
        {
            get => _myStory;
            set { _myStory = value; OnPropertyChanged(); }
        }

        public string Location
        {
            get => _location;
            set { _location = value; OnPropertyChanged(); }
        }

        public string Region
        {
            get => _region;
            set { _region = value; OnPropertyChanged(); }
        }

        public string HeadTitle
        {
            get => _headTitle;
            set
            {
                _headTitle = value;
                OnPropertyChanged();
            }
        }
        #endregion
        public CreateStoryPage()
        {
            BindingContext = this;
            InitializeComponent();
        }


        private async void ChooseBanner(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("No pickfoto", "No pickfoto available", "ok");
                return;
            }

            _mediaFile = await CrossMedia.Current.PickPhotoAsync();
            if (_mediaFile == null) return;
            BannerImage.Source = ImageSource.FromStream(() => _mediaFile.GetStream());
        }

        private async void CreateStory(object sender, EventArgs e)
        {
            IsBusy = true;

            await UploadImage(_mediaFile);
            var up = await _ups.GetProfileAsync(Settings.AccessToken);

            var story = new Models.Story()
            {
                BannerImage = CoverImage,
                HeadTitle = HeadTitle,
                HeadLines = StoryText.SafeSubstring(0,20)+"...",
                StoryText = StoryText,
                Location = Location,
                Region = Region,
                Rating = Rating,
                UserProfileModelId = up.Id
            };
            var storyId = await _storyServices.PostStoryAsync(Settings.AccessToken, story);
            IsBusy = false;
            //if all good send user to gallery with the id = Convert.ToInt32(storyId);

            await Navigation.PushModalAsync(new CreateGalleryStory(Convert.ToInt32((string) storyId)));
        }

        public async Task<string> UploadImage(MediaFile mediaFile)
        {
            var uploadPhoto = await _storyServices.UploadPhoto(Settings.AccessToken, mediaFile);
            CoverImage = uploadPhoto;
            return uploadPhoto;
        }
    }
}