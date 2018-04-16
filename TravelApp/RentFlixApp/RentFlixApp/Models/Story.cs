using System;
using System.Collections.Generic;
using System.Text;

namespace RentFlixApp.Models
{
    public class Story
    {      
        public int Id { get; set; }
        public UserProfileModel UserProfileModel { get; set; }
        public int UserProfileModelId { get; set; }
        public DateTime CreateDate { get; set; }
        public string HeadTitle { get; set; }
        public string HeadLines { get; set; }
        public string StoryText { get; set; }
        public string Region { get; set; }
        public string Location { get; set; }
        public int Rating { get; set; }
        public string BannerImage { get; set; }
        public int TotalLikes { get; set; }
        public int TotalComments { get; set; }
        public int TotalPictures { get; set; }
        public List<Gallery> Photos { get; set; }
        public List<Comment> Comments { get; set; }

    }
}
