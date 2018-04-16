using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using RentFlixApp.Annotations;
using RentFlixApp.Helpers;
using RentFlixApp.Models;
using RentFlixApp.Services;

namespace RentFlixApp.ViewModels
{
    public class UserProfileViewModel : INotifyPropertyChanged
    {
        private readonly UserProfileService _registerServices = new UserProfileService();
        #region properties
        private bool _isBusy;
        private List<UserProfile> _usersProfiles;
        private string _fullName;
        private string _dateOfBirth;
        private string _avatar;
        private string _aboutMe;
        private string _email;
        private string _joinedDate;

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

        public List<UserProfile> UsersProfiles
        {
            get => _usersProfiles;
            set
            {
                _usersProfiles = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        public async Task<string> UploadImage(MediaFile mediaFile)
        {
            var uploadPhoto = await _registerServices.UploadPhoto(Settings.AccessToken, mediaFile);
            Avatar = uploadPhoto;
            return uploadPhoto;
        }
        public async Task<bool> CreateUserProfile()
        {
            var userProfile = new UserProfile
            {
                FullName = FullName,
                AboutMe = AboutMe,
                DateOfBirth = Convert.ToDateTime(DateOfBirth),
                Avatar = Avatar
            };
            IsBusy = true;
            var isSuccess = await _registerServices.PostUserProfileAsync(userProfile, Settings.AccessToken);
            IsBusy = false;
            return isSuccess;
        }

        public async Task<UserProfile> ReadUserProfile()
        {
            var profile = await _registerServices.GetProfileAsync(Settings.AccessToken);
            return profile;
        }

        public async Task ReadUsersProfiles()
        {
            UsersProfiles = await _registerServices.GetAllProfilesAsync(Settings.AccessToken);
        }

        public async Task UpdateUserProfile(UserProfile profile)
        {
           await _registerServices.UpdateProfileAsync(profile, Settings.AccessToken);
        }
    }
}
