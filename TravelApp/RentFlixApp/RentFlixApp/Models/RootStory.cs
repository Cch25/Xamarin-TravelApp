using System;
using System.Collections.Generic;
using System.Text;

namespace RentFlixApp.Models
{
    public class RootStory
    {
        public int TotalStories { get; set; }
        public List<Story> Stories { get; set; }
    }
}
