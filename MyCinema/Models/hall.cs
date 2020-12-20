using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCinema.Models
{
    public class hall
    {
        public string ID { get; set; }

        public string Hall_Name { get; set; }
        //public string Seats_Num { get; set; }
        public List<hall> hall_details { get; set; }
    }
}