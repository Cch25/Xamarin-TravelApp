using System;
using RentFlixApp.Helpers;
using RentFlixApp.Models;
using RentFlixApp.Services;
using RentFlixApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentFlixApp.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfilePage : ContentPage
    {
        #region proprietes
        public UserProfilePostsAndRatings UserProfilePostsAndRatings { get; set; }
        private readonly UserProfileService _userProfileService = new UserProfileService();
        private string _fullName;
        private string _dateOfBirth;
        private string _avatar;
        private string _aboutMe;
        private string _email;
        private string _joinedDate;
        private int _totalPosts;
        private int _totalStars;

        public int TotalStars
        {
            get => _totalStars;
            set { _totalStars = value; OnPropertyChanged(); }
        }

        public int TotalPosts
        {
            get => _totalPosts;
            set { _totalPosts = value; OnPropertyChanged(); }
        }


        public string JoinedDate
        {
            get => _joinedDate;
            set
            {
                _joinedDate = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string AboutMe
        {
            get => _aboutMe;
            set { _aboutMe = value; OnPropertyChanged(); }
        }

        public string Avatar
        {
            get => _avatar;
            set { _avatar = value; OnPropertyChanged(); }
        }

        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged();
            }
        }

        public string DateOfBirth
        {
            get => _dateOfBirth;
            set { _dateOfBirth = value; OnPropertyChanged(); }
        }


        #endregion
        public UserProfilePage()
        {
            BindingContext = this;
            InitializeComponent();
        }

        protected override async void OnAppearing() //bug - duplicate tabbed page
        {
            base.OnAppearing();
            IsBusy = true;
            var checkprofile = await _userProfileService.GetProfileAsync(Settings.AccessToken);
            if (checkprofile == null)
            {
                await Navigation.PushAsync(new CreateProfilePage());
            }
            else
            {
                var profile = await _userProfileService.GetSpecificProfile(Settings.AccessToken, checkprofile.Id);
                FullName = checkprofile.FullName;
                Avatar = Constants.BaseUrlNoSlash + profile.ProfileDto.Avatar.Replace("\"", "");
                AboutMe = profile.ProfileDto.AboutMe;
                Email = profile.ProfileDto.UserDto.Email;
                TotalPosts = profile.TotalPosts;
                TotalStars = profile.TotalRatings;
                JoinedDate = profile.ProfileDto.UserDto.JoinedDate.ToShortDateString();
                DateOfBirth = profile.ProfileDto.DateOfBirth.ToShortDateString();
                IsBusy = false;
            }
            IsBusy = false;
        
        }

        private void Edit_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UpdateProfilePage());
        }

    }
}