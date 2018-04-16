using System;

namespace RentFlixApp.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public DateTime JoinedDate { get; set; }
        public string UserName { get; set; }
    }
}
