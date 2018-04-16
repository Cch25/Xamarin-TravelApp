using System;
using RentFlixApp.Models;
using RentFlixApp.Pages;
using RentFlixApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentFlixApp.Views.Trendings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MostAppreciated : ContentPage
    {
        public MostAppreciated()
        {
            ViewModel = new CardDataViewModel();
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            ViewModel.GenerateArrangedDataCollection(0,10);
            base.OnAppearing();
        }

        public CardDataViewModel ViewModel
        {
            get => BindingContext as CardDataViewModel;
            set => BindingContext = value;
        }
        private void RefreshList(object sender, EventArgs e)
        {
            ViewModel.GenerateArrangedDataCollection(0,10);
            ListView.EndRefresh();
        }

        private async void ListView_OnItemTappedtemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is CardDataModel story)
                await Navigation.PushAsync(new StoryTabbedPage(story.PostId));
        }
    }
}