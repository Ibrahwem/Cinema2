using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyCinema.Models
{
    public class BookingTable
    {
        public int Id { get; set; }
        public string seatno { get; set; }
        public string UserId { get; set; }
        public string Datetopresent { get; set; }
        public string Timetopresent { get; set; }

        public int MovieDetailsId { get; set; }
        public string Amount { get; set; }
        [ForeignKey("MovieDetailsId")]
        public virtual Movy moviedetails { get; set; }


    }
}