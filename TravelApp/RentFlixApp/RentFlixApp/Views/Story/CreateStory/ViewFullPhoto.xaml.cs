using System;
using RentFlixApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RentFlixApp.Views.Story.CreateStory
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewFullPhoto : ContentPage
    {
        #region private properties
        private string _description;
        private string _imagePath;

        public string ImagePath
        {
            get => _imagePath;
            set { _imagePath = value; OnPropertyChanged();}
        }


        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged();}
        }

        #endregion
        public ViewFullPhoto(Gallery gallery)
        {
            Description = $"\t\t{gallery.Description}";
            ImagePath = gallery.FullPath;
            BindingContext = this;
            InitializeComponent();
        }

        private async void GoBackToGallery(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}