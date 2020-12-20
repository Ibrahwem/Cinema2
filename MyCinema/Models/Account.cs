using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyCinema.Models
{
    public class Account
    {
        [Required(ErrorMessage = "This field is required.")]

        public string username { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DataType(DataType.Password)]

        public string password { get; set; }

    }
}