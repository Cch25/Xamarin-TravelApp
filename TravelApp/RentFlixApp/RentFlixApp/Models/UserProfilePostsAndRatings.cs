using System;
using System.Collections.Generic;
using System.Text;

namespace RentFlixApp.Models
{
    public class UserProfilePostsAndRatings
    {
        public int TotalPosts { get; set; }
        public int TotalRatings { get; set; }
        public UserProfile ProfileDto { get; set; }

    }
}
