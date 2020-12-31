using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCinema.Models
{
    public class BookSeat
    {
        public int Id { get; set; }
        public string movieName { get; set; }
        public string moviedate { get; set; }
        public string movietime { get; set; }
        public string seatno { get; set; }



    }
}