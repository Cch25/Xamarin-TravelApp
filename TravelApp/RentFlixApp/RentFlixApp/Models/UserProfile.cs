using System;

namespace RentFlixApp.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public User UserDto { get; set; }
        public object UserDtoId { get; set; }
        public string AboutMe { get; set; }
        public string Avatar { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
