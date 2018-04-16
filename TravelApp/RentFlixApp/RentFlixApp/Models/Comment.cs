using System;
using System.Collections.Generic;

namespace RentFlixApp.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int StoriesModelId { get; set; }
        public string CommentText { get; set; }
        public DateTime DatePosted { get; set; }
        public UserProfile UserProfile { get; set; }
        public int UserProfileId { get; set; }
    }
    public class RootComment
    {
        public int TotalComments { get; set; }
        public List<Comment> Comments { get; set; }
    }
}