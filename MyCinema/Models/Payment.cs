using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyCinema.Models
{
    public class Payment
    {
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(16, MinimumLength = 16)]
        [DisplayName("Visa Number")]
        public string Number { get; set; }
        public string tokef { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(3,MinimumLength =3)]
        public string cvv { get; set; }
        [Required(ErrorMessage = "This field is required.")]

        public string Amount { get; set; }

    }
}