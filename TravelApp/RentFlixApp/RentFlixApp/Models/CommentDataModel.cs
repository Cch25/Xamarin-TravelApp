namespace RentFlixApp.Models
{
    public class CommentDataModel
    {
        public int UserProfileId { get; set; } //to navigate to the user's profile.
        public string CommentText { get; set; }
        public string DatePosted { get; set; }
        public string AvatarImage { get; set; }
        public string FullName { get; set; }
    }
}
