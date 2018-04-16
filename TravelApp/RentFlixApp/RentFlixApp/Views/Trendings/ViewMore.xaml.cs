using System;
using RentFlixApp.Models;
using RentFlixApp.Pages;
using RentFlixApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentFlixApp.Views.Trendings
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewMore : ContentPage
	{
		public ViewMore ()
		{
            ViewModel = new CardDataViewModel();
			InitializeComponent ();
		}
	    public CardDataViewModel ViewModel
	    {
	        get => BindingContext as CardDataViewModel;
	        set => BindingContext = value;
	    }

	    protected override void OnAppearing()
	    {
	        ViewModel.GenerateArrangedDataCollection(10, 100);
            base.OnAppearing();
	    }

	    private void RefreshList(object sender, EventArgs e)
	    {
	        ViewModel.GenerateArrangedDataCollection(10, 100);
	        ListView.EndRefresh();
        }

	    private async void ListView_OnItemTappedtemTapped(object sender, ItemTappedEventArgs e)
	    {
	        if (e.Item is CardDataModel story)
	            await Navigation.PushAsync(new StoryTabbedPage(story.PostId));
        }
	}
}