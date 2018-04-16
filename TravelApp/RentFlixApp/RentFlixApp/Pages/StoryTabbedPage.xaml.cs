using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CommentsStory = RentFlixApp.Views.Story.ReadStory.CommentsStory;
using GalleryStory = RentFlixApp.Views.Story.ReadStory.GalleryStory;
using ReadCompleteStory = RentFlixApp.Views.Story.ReadStory.ReadCompleteStory;

namespace RentFlixApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StoryTabbedPage : TabbedPage
	{
		public StoryTabbedPage(int storyPostId)
		{
		    InitializeComponent();
		    Title = "Explore story";
            var readStory = new ReadCompleteStory()
		    {
		        Icon = "storyteller.png",
		        //Title = "Details",
                PostId = storyPostId
		    };
		    var commentStory = new CommentsStory()
		    {
		        Icon = "commentreviews.png",
		        //Title = "Reviews",
                PostId = storyPostId
            };
		    var galleryStory = new GalleryStory()
		    {
		        Icon = "gallery.png",
		        //Title = "Photos",
                PostId = storyPostId
            };
		    Children.Add(readStory);
		    Children.Add(galleryStory);
		    Children.Add(commentStory);
        }

	}
}