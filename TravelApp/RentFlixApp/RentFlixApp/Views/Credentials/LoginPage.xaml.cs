using System;
using RentFlixApp.Helpers;
using RentFlixApp.Pages;
using RentFlixApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentFlixApp.Views.Credentials
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {

        public LoginPage()
        {
            ViewModel = new LoginViewModel();
            InitializeComponent();
        }

        private async void GoToMain(object sender, EventArgs e)
        {
            await ViewModel.LoginUserAsync();
            if (!string.IsNullOrEmpty(Settings.AccessToken))
                await Navigation.PushModalAsync(new MasterMenu());
            else
                await DisplayAlert("Failed to log in", "Wrong username or password", "Try again");
        }

        private async void Handle_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new RegisterPage());
        }

        public LoginViewModel ViewModel
        {
            get => BindingContext as LoginViewModel;
            set => BindingContext = value;
        }
    }
}