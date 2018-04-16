using System;
using System.Threading.Tasks;
using RentFlixApp.Helpers;
using RentFlixApp.Models;
using RentFlixApp.Services;
using RentFlixApp.ViewModels;
using RentFlixApp.Views.Profile;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentFlixApp.Views.Story.ReadStory
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReadCompleteStory : ContentPage
    {
        #region Properties
        private readonly StoryServices _storyServices = new StoryServices();
        private int _postId;
        private string _headTitle;
        private string _region;
        private string _bannerImage;
        private string _location;
        private string _storyText;
        private int _rating;
        private string _userImage;
        private string _userFullName;
        private string _datePosted;
        private int _userProfileId;
        private int _likeId;
        private int _pageCreatorId;
        public int UserProfileId
        {
            get => _userProfileId;
            set { _userProfileId = value; OnPropertyChanged(); }
        }

        public string DatePosted
        {
            get => _datePosted;
            set { _datePosted = value; OnPropertyChanged(); }
        }


        public string UserFullName
        {
            get => _userFullName;
            set { _userFullName = value; OnPropertyChanged(); }
        }

        public string UserImage
        {
            get => _userImage;
            set { _userImage = value; OnPropertyChanged(); }
        }

        public int Rating
        {
            get => _rating;
            set { _rating = value; OnPropertyChanged(); }
        }


        public string StoryText
        {
            get => _storyText;
            set { _storyText = value; OnPropertyChanged(); }
        }


        public string Location
        {
            get => _location;
            set { _location = value; OnPropertyChanged(); }
        }


        public string BackImage
        {
            get => _bannerImage;
            set { _bannerImage = value; OnPropertyChanged(); }
        }

        public int PostId
        {
            get => _postId;
            set { _postId = value; OnPropertyChanged(); }
        }

        public string HeadTitle
        {
            get => _headTitle;
            set { _headTitle = value; OnPropertyChanged(); }
        }
        public string Region
        {
            get => _region;
            set { _region = value; OnPropertyChanged(); }
        }
        #endregion
        public ReadCompleteStory()
        {
            BindingContext = this;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {

            IsBusy = true;

            var story = await _storyServices.GetStoryAsync(Settings.AccessToken, PostId);
            HeadTitle = story.HeadTitle;
            Region = story.Region;
            BackImage = Constants.BaseUrlNoSlash + story.BannerImage;
            Location = story.Location;
            StoryText = story.StoryText;
            Rating = story.Rating;
            UserImage = Constants.BaseUrlNoSlash + story.UserProfileModel.Avatar;
            UserFullName = story.UserProfileModel.FullName;
            DatePosted = CardDataViewModel.ToHumanDate(story.CreateDate);
            _pageCreatorId = story.UserProfileModelId;
            //display the heart with like or dislike at the begining
            await DisplayLikedDisliked();
            //JustifyText(story.StoryText);
            IsBusy = false;
            base.OnAppearing();
        }

        //private void JustifyText(string storyText)
        //{
        //    var browser = new WebView
        //    {
        //        HorizontalOptions = LayoutOptions.FillAndExpand,
        //        VerticalOptions = LayoutOptions.FillAndExpand
        //    };

        //    var source = new HtmlWebViewSource();
        //    var text = "<html>" +
        //               "<body  style=\"text-align: justify;\">" +
        //               string.Format("<p><i>,,{0}\'\'</i></p>", storyText) +
        //               "</body>" +
        //               "</html>";
        //    source.Html = text;
        //    browser.Source = source;
        //    StoryBehind.Children.Add(browser);
        //}
        private async void HeartClicked(object sender, EventArgs e)
        {
            await CreateLikeOrDislike();
        }

        private async void LikedClicked(object sender, EventArgs e)
        {
            await CreateLikeOrDislike();
        }

        private async Task<bool> CheckIfLiked()
        {
            var ups = new UserProfileService();
            var profile = await ups.GetProfileAsync(Settings.AccessToken);
            UserProfileId = profile.Id;

            var result = await _storyServices.GetStoryLike(Settings.AccessToken, PostId, UserProfileId);
            if (result == null) return false;
            _likeId = result.Id;
            return true;

        }

        private async Task DisplayLikedDisliked()
        {
            var liked = await CheckIfLiked();
            if (liked)
            {
                LikePostImage.Source = "likedButton.png";
                LikePostLabel.Text = "Liked";
            }
            else
            {
                LikePostImage.Source = "dislikedButton.png";
                LikePostLabel.Text = "Like";
            }
        }

        private async Task CreateLikeOrDislike()
        {
            IsBusy = true;
            var liked = await CheckIfLiked();

            var storyLike = new StoryLike()
            {
                UserProfileId = UserProfileId,
                StoryModelId = PostId
            };

            if (!liked) //if not liked and you press the like
            {
                LikePostImage.Source = "likedButton.png";
                LikePostLabel.Text = "Liked";
                await _storyServices.AddStoryLike(Settings.AccessToken, storyLike);
            }
            else
            {
                LikePostImage.Source = "dislikedButton.png";
                LikePostLabel.Text = "Like";
                await _storyServices.DeleteStoryLikeAsync(Settings.AccessToken, _likeId);
            }

            IsBusy = false;
        }

        private async void UserImageTapped(object sender, EventArgs e)
        {
            await ViewStoryCreatorProfile();
        }

        private async void UserNameTapped(object sender, EventArgs e)
        {
            await ViewStoryCreatorProfile();
        }

        private async Task ViewStoryCreatorProfile()
        {
            IsBusy = true;
            var ups = new UserProfileService();
            var profile = await ups.GetSpecificProfile(Settings.AccessToken, _pageCreatorId);
            await Navigation.PushAsync(new ReadUserProfilePage(profile));
            IsBusy = true;
        }
    }
}