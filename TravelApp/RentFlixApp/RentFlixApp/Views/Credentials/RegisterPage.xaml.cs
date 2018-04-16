using System;
using RentFlixApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentFlixApp.Views.Credentials
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            ViewModel = new RegisterViewModel();
            InitializeComponent();
        }

        private async void RedirectToLogin(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoginPage());
        }

        private async void CheckCredentials(object sender, EventArgs e)
        {
            var isSuccess = await ViewModel.RegisterUser();
            if (isSuccess)
            {
                await DisplayAlert("Status", "Your account is created! \nYou can log in now.", "Ok");
                RedirectToLogin(this, EventArgs.Empty);
            }
            else
                await DisplayAlert("Status", "Something went wrong here", "Try Again");
        }

        public RegisterViewModel ViewModel
        {
            get => BindingContext as RegisterViewModel;
            set => BindingContext = value;
        }

    }
}