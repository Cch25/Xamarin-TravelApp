using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RentFlixApp.Helpers;
using RentFlixApp.Models;
using RentFlixApp.ViewModels;
using RentFlixApp.Views.Story;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CreateStoryPage = RentFlixApp.Views.Story.CreateStory.CreateStoryPage;
using LoginPage = RentFlixApp.Views.Credentials.LoginPage;

namespace RentFlixApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterMenu : MasterDetailPage
    {
        #region properties
        public List<MainMenuItem> MainMenuItems { get; set; }
        public string FullName
        {
            get => _fullName;
            set { _fullName = value; OnPropertyChanged(); }
        }

        public string AvatarImage
        {
            get => _avatarImage;
            set { _avatarImage = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        private string _email;
        private UserProfileViewModel _upvm;
        private string _fullName;
        private string _avatarImage;
        #endregion

        public MasterMenu()
        {
            BindingContext = this;

            CheckIfUserHasProfile();

            MainMenuItems = new List<MainMenuItem>()
            {
                new MainMenuItem() { Title = "Top stories", Icon = "home.png", TargetType = typeof(TrendingsTabbedPage) },
                new MainMenuItem() { Title = "Stories", Icon = "search.png", TargetType = typeof(MainStoriesPage) },
                new MainMenuItem() { Title = "Create story",Icon = "create.png",TargetType = typeof(CreateStoryPage)},
                new MainMenuItem() { Title = "Profiles", Icon = "profile.png", TargetType = typeof(UserProfileTabbedPages)}
            };

            Detail = new NavigationPage(new MainStoriesPage());
            InitializeComponent();
        }

        private async void CheckIfUserHasProfile()
        {
            IsBusy = true;
            _upvm = new UserProfileViewModel();
            var profile = await _upvm.ReadUserProfile();
            if (profile != null)
            {
                FullName = profile.FullName;
                AvatarImage = Constants.BaseUrlNoSlash + profile.Avatar;
                Email = profile.UserDto.Email;
            }
            else
            {
                FullName = "Traveler";
                AvatarImage = "logo.png";
            }

            IsBusy = false;
        }
        public void MainMenuItem_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is MainMenuItem item)
            {
                if (item.Title.Equals("Top stories"))
                {
                    Detail = new NavigationPage(new TrendingsTabbedPage());
                }
                else if (item.Title.Equals("Stories"))
                {
                    Detail = new NavigationPage(new MainStoriesPage());
                }
                else if (item.Title.Equals("Profiles"))
                {
                    Detail = new NavigationPage(new UserProfileTabbedPages());
                }
                else if (item.Title.Equals("Create story"))
                {
                    Detail = new NavigationPage(new CreateStoryPage());
                }

                MenuListView.SelectedItem = null;
                IsPresented = false;
            }
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            Settings.AccessToken = "";
            await Navigation.PushModalAsync(new LoginPage());
        }
    }
}