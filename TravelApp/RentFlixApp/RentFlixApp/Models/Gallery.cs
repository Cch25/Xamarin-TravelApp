namespace RentFlixApp.Models
{
    public class Gallery
    {
        public int Id { get; set; }
        public int StoriesModelId { get; set; }
        public string FullPath { get; set; }
        public string Description { get; set; }
    }
}
