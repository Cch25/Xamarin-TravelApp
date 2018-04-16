using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RentFlixApp.Helpers;
using RentFlixApp.Models;
using RentFlixApp.Services;
using RentFlixApp.ViewModels;
using RentFlixApp.Views.Profile;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentFlixApp.Views.Story.ReadStory
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommentsStory : ContentPage
    {
        #region private propreties 
        private readonly StoryServices _storyServices = new StoryServices();
        private int _postId;
        private string _userName;
        private string _userFullName;
        private IList<CommentDataModel> _cardDataCollection;
        private string _addComment;
        private bool _noComment;

        public bool NoComment
        {
            get => _noComment;
            set { _noComment = value; OnPropertyChanged(); }
        }


        public string AddComment
        {
            get => _addComment;
            set { _addComment = value; OnPropertyChanged(); }
        }


        public IList<CommentDataModel> CardDataCollection
        {
            get => _cardDataCollection;
            set { _cardDataCollection = value; OnPropertyChanged(); }
        }

        public string UserFullName
        {
            get => _userFullName;
            set { _userFullName = value; OnPropertyChanged(); }
        }

        public string UserImage
        {
            get => _userName;
            set { _userName = value; OnPropertyChanged(); }
        }

        public int PostId
        {
            get => _postId;
            set { _postId = value; OnPropertyChanged(); }
        }

        #endregion
        public CommentsStory()
        {
            BindingContext = this;
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            IsBusy = true;
            await LoadCommentsAsync();
            IsBusy = false;
            base.OnAppearing();
        }

        private async Task LoadCommentsAsync()
        {
            var comments = await _storyServices.GetStoryComments(Settings.AccessToken, PostId);
            CardDataCollection = new ObservableCollection<CommentDataModel>();
            NoComment = comments.Comments.Count <= 0;

            foreach (var comment in comments.Comments)
            {
                CardDataCollection.Insert(0, new CommentDataModel
                {
                    UserProfileId = comment.UserProfile.Id,
                    CommentText = comment.CommentText,
                    DatePosted = CardDataViewModel.ToHumanDate(comment.DatePosted),
                    FullName = comment.UserProfile.FullName,
                    AvatarImage = Constants.BaseUrlNoSlash + comment.UserProfile.Avatar
                });
            }
        }

        private async void SubmitComment(object sender, EventArgs e)
        {
            IsBusy = true;

            var ups = new UserProfileService();
            var currentProfile = await ups.GetProfileAsync(Settings.AccessToken);

            //persist comment in database
            var persistComment = new Comment()
            {
                UserProfileId = currentProfile.Id,
                CommentText = AddComment,
                StoriesModelId = PostId,
            };
            await _storyServices.AddStoryComment(Settings.AccessToken, persistComment);
            IsBusy = false;

            //add comment to list
            var addComment = new CommentDataModel()
            {
                FullName = currentProfile.FullName,
                DatePosted = CardDataViewModel.ToHumanDate(DateTime.Now),
                CommentText = AddComment,
                AvatarImage = Constants.BaseUrlNoSlash + currentProfile.Avatar
            };
            CardDataCollection.Insert(0, addComment);

            //clear textbox
            AddComment = "";
        }

        private async void RefreshList(object sender, EventArgs e)
        {
            await LoadCommentsAsync();
            ListViewComments.ItemsSource = CardDataCollection;
            ListViewComments.EndRefresh();
        }

        private async void VisitProfile(object sender, ItemTappedEventArgs e)
        {
            IsBusy = true;
            var tappedProfile = e.Item as CommentDataModel;
            var ups = new UserProfileService();

            if (tappedProfile != null)
            {
                var profile = await ups.GetSpecificProfile(Settings.AccessToken, tappedProfile.UserProfileId);
                await Navigation.PushAsync(new ReadUserProfilePage(profile));
            }
            IsBusy = false;
        }
    }
}