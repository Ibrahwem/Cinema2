using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCinema.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string seatno { get; set; }
        public string UserId { get; set; }

        public string date { get; set; }
        public string time { get; set; }


        public string Amount { get; set; }
        public int MovieId { get; set; }
        

    }
}