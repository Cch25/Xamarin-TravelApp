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
    public partial class BrowseProfilesPage : ContentPage
    {

        public BrowseProfilesPage()
        {
            ViewModel = new UserProfileViewModel();
            InitializeComponent();

        }

        protected override async void OnAppearing()
        {
            ViewModel.IsBusy = true;
            await ViewModel.ReadUsersProfiles();
            ViewModel.IsBusy = false;
            base.OnAppearing();
        }

        private async void UserProfileView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var ups = new UserProfileService();
            if (e.Item is UserProfile item)
            {
                var profile = await ups.GetSpecificProfile(Settings.AccessToken, item.Id);
                await Navigation.PushAsync(new ReadUserProfilePage(profile));
            }
        }

        private async void RefreshList(object sender, EventArgs e)
        {
            await ViewModel.ReadUsersProfiles();
            UsersProfile.EndRefresh();
        }

        public UserProfileViewModel ViewModel
        {
            get => BindingContext as UserProfileViewModel;
            set => BindingContext = value;
        }
    }
}