using System;
using RentFlixApp.Models;
using RentFlixApp.Pages;
using RentFlixApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentFlixApp.Views.Story
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainStoriesPage : ContentPage
    {
        public MainStoriesPage()
        {
            ViewModel = new CardDataViewModel();
            InitializeComponent();
        }

        private async void ListView_OnItemTappedtemTapped(object sender, ItemTappedEventArgs e)
        {
 
            if (e.Item is CardDataModel story)
                await Navigation.PushAsync(new StoryTabbedPage(story.PostId));

        }

        private void RefreshList(object sender, EventArgs e)
        {
            ViewModel.GenerateCardModel();
            ListView.EndRefresh();
        }

        private void SearchButtonClicked(object sender, EventArgs e)
        {
            ViewModel.IsSearchVisible = !ViewModel.IsSearchVisible;
            SearchEntry.Focus();
        }

        public CardDataViewModel ViewModel
        {
            get => BindingContext as CardDataViewModel;
            set => BindingContext = value;
        }
        private void SearchStories(object sender, EventArgs e)
        {
            ViewModel.SearchCardViewModels(SearchEntry.Text);
            SearchEntry.Text = "";
            ViewModel.IsSearchVisible = false;
        }
    }
}