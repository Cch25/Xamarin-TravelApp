using System;
using System.Threading.Tasks;
using RentFlixApp.Helpers;
using RentFlixApp.Models;
using RentFlixApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentFlixApp.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReadUserProfilePage : ContentPage
    {
        #region properties
        private readonly StoryServices _storyServices = new StoryServices();
        public string AboutMe { get; set; }
        public string Avatar { get; set; }
        public int UserProfileId { get; set; }
        public string FullName { get; set; }
        public string DateOfBirth { get; set; }
        public string Email { get; set; }
        public string JoinedDate { get; set; }
        public int Posts { get; set; }
        public int Stars { get; set; }
        public int CurrentUserProfileId { get; set; }
        #endregion
        public ReadUserProfilePage(UserProfilePostsAndRatings profile)
        {
            BindingContext = this;
            IsBusy = true;
            AboutMe = profile.ProfileDto.AboutMe;
            Avatar = Constants.BaseUrlNoSlash + profile.ProfileDto.Avatar.Replace("\"", "");
            FullName = profile.ProfileDto.FullName;
            DateOfBirth = Convert.ToDateTime(profile.ProfileDto.DateOfBirth).ToShortDateString();
            Email = profile.ProfileDto.UserDto.Email;
            JoinedDate = profile.ProfileDto.UserDto.JoinedDate.ToShortDateString();
            Posts = profile.TotalPosts;
            Stars = profile.TotalRatings;
            UserProfileId = profile.ProfileDto.Id;
            IsBusy = false;
            InitializeComponent();

        }

        protected override async void OnAppearing()
        {
            var hasBeenStarred = await CheckIfUserWasAlreadyStarred();
            if (hasBeenStarred)
            {
                DisableStarButton();
            }

            base.OnAppearing();
        }

        private async void Rate_Clicked(object sender, EventArgs e)
        {
            IsBusy = true;
            var result = await DisplayAlert("Star profile", "Are you sure you want to star this profile?", "Ok", "Cancel");
            if (result)
            {
                var stars = new Stars()
                {
                    UserWhoSent = CurrentUserProfileId,
                    UserWhoReceived = UserProfileId
                };
                await _storyServices.AddStar(Settings.AccessToken, stars);
                DisableStarButton();
                StarLabel.Text = (Stars+1).ToString();
            }

            IsBusy = false;
        }

        private async Task<bool> CheckIfUserWasAlreadyStarred()
        {
            IsBusy = true;
            var ups = new UserProfileService();
            var currentUser = await ups.GetProfileAsync(Settings.AccessToken);
            CurrentUserProfileId = currentUser.Id;
            var userStarred = await _storyServices.GetProfileStars(Settings.AccessToken, currentUser.Id, UserProfileId);
            IsBusy = false;
            return userStarred != null;
        }

        private void DisableStarButton()
        {
            StarButton.IsEnabled = false;
            StarButton.BackgroundColor = Color.LimeGreen;
            StarButton.TextColor = Color.White;
            StarButton.Text = "Already starred by you";
        }
    }
}