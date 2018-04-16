using System;
using Plugin.Media;
using Plugin.Media.Abstractions;
using RentFlixApp.Models;
using RentFlixApp.Pages;
using RentFlixApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentFlixApp.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateProfilePage : ContentPage
    {
        #region properties
        private string _fullName;
        private string _avatar;
        private string _aboutMe;
        private DateTime _dateOfBirth;
        public string FullName
        {
            get => _fullName;
            set { _fullName = value; OnPropertyChanged(); }
        }

        public string Avatar
        {
            get => _avatar;
            set { _avatar = value; OnPropertyChanged(); }
        }

        public string AboutMe
        {
            get => _aboutMe;
            set { _aboutMe = value; OnPropertyChanged(); }
        }


        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set { _dateOfBirth = value; OnPropertyChanged(); }
        }

        #endregion
        public UpdateProfilePage()
        {
            BindingContext = this;
            GetData();
            InitializeComponent();
        }
        private MediaFile _mediaFile;
        private readonly UserProfileViewModel _upvm = new UserProfileViewModel();
        private UserProfile _userProfile;
        private string _newAvatarPath = "";

        private async void GetData()
        {
            _userProfile = await _upvm.ReadUserProfile();
            FullName = _userProfile.FullName;
            Avatar = Constants.BaseUrlNoSlash + _userProfile.Avatar;
            _newAvatarPath = _userProfile.Avatar;
            AboutMe = _userProfile.AboutMe;
            DateOfBirth = _userProfile.DateOfBirth;
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
            AvatarImage.Source = ImageSource.FromStream(() => _mediaFile.GetStream());
            _newAvatarPath = await _upvm.UploadImage(_mediaFile);        
        }

        private void DatePicker_OnDateSelected(object sender, DateChangedEventArgs e)
        {
            DateOfBirth = e.NewDate;
        }

        private async void UpdateProfile(object sender, EventArgs e)
        {
            _userProfile.FullName = FullName;
            _userProfile.AboutMe = AboutMe;
            _userProfile.DateOfBirth = DateOfBirth;
            _userProfile.Avatar = _newAvatarPath.Replace("\"",""); //fixed
            IsBusy = true;
            await _upvm.UpdateUserProfile(_userProfile);
            IsBusy = false;
            await DisplayAlert("Info", "Profile updated", "Ok");
            await Navigation.PushModalAsync(new MasterMenu());
        }

    }
}