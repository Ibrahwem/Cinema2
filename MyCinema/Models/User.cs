using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyCinema.Models
{
    public class User
    {
        [Required(ErrorMessage = "This field is required.")]
        public int UserID { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string username { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [DataType(DataType.Password)]

        public string password { get; set; }
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [Compare("password")]
        public string ConfirmPassword { get; set; }

    }
}