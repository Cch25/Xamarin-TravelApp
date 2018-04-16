using System;
using Plugin.Media;
using Plugin.Media.Abstractions;
using RentFlixApp.Pages;
using RentFlixApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentFlixApp.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateProfilePage : ContentPage
    {
        private MediaFile _mediaFile;
        public CreateProfilePage()
        {
            ViewModel = new UserProfileViewModel();
            InitializeComponent();
        }

        private async void CreateUserProfile(object sender, EventArgs e)
        {
            ViewModel.IsBusy = true;
            await ViewModel.UploadImage(_mediaFile);
            var isSuccess = await ViewModel.CreateUserProfile();
            ViewModel.IsBusy = false;

            if (isSuccess)
            {
                await DisplayAlert("Status", "Profile created!", "Ok");
                await Navigation.PushModalAsync(new MasterMenu());
            }
            else
                await DisplayAlert("Status", "Something went wrong here", "Try Again");

        }

        private async void Handle_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new MasterMenu());
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
        }



        private void DatePicker_OnDateSelected(object sender, DateChangedEventArgs e)
        {
            ViewModel.DateOfBirth = e.NewDate.ToShortDateString();
        }

        public UserProfileViewModel ViewModel
        {
            get => BindingContext as UserProfileViewModel;
            set => BindingContext = value;
        }
    }
}