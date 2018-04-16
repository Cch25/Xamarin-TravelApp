using System;
using System.Collections.Generic;
using System.Text;

namespace RentFlixApp.Models
{
    public class Stars
    {
        public int Id { get; set; }
        public int UserWhoSent { get; set; }
        public int UserWhoReceived { get; set; }
    }
}
