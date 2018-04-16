using System;
using System.Collections.Generic;
using System.Text;

namespace RentFlixApp.Models
{
    public class RegisterBindingModel
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
