using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCinema.Models
{
    public class BookNowViewModel
    {
        public string Movie_Name { get; set; }
        public string date { get; set; }

        public string time { get; set; }
        public string SeatNo { get; set; }
        public string Amount { get; set; }
        public string hall { get; set; }
        public string User_Name { get; set; }


        public int MovieId { get; set; }

    }
}