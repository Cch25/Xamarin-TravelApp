using System;

namespace RentFlixApp.Models
{
    public class UserProfileModel
    {
        public int Id { get; set; }
        public object ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public string AboutMe { get; set; }
        public string Avatar { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}