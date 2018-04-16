using System;
using RentFlixApp.Helpers;
using RentFlixApp.Pages;
using RentFlixApp.ViewModels;
using RentFlixApp.Views;

using Xamarin.Forms;
using LoginPage = RentFlixApp.Views.Credentials.LoginPage;
using RegisterPage = RentFlixApp.Views.Credentials.RegisterPage;

namespace RentFlixApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new NavigationPage(new CreateProfilePage());
            SetMainPage();

        }

        private void SetMainPage()
        {
            if (!string.IsNullOrEmpty(Settings.AccessToken))
            {
                if (DateTime.UtcNow.AddHours(2) < Settings.AccessTokenExpiration)
                {
                    var vm = new LoginViewModel();
                    vm.LoginCommand.Execute(null);
                }
                //MainPage = new NavigationPage(new MasterMenu()); //fixed duplicate masterdetail page
                MainPage = new MasterMenu();
            }
            else if (!string.IsNullOrEmpty(Settings.Username) &&
                     !string.IsNullOrEmpty(Settings.AccessToken))
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                MainPage = new NavigationPage(new RegisterPage());
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
