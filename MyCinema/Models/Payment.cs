using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyCinema.Models
{
    public class Payment
    {
        [Required(ErrorMessage = "This field is required.")]
        public string Number { get; set; }
        public string tokef { get; set; }
        [Required(ErrorMessage = "This field is required.")]

        public string cvv { get; set; }
        [Required(ErrorMessage = "This field is required.")]

        public string Amount { get; set; }

    }
}