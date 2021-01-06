using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCinema.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Movie's Name")]

        public string Name { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "age limitation")]

        public string age { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Category")]
        public string Category { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Movie Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Movie's Date")]
        [DataType(DataType.Date)]
      // [DisplayFormat(DataFormatString ="{0:Day/Mont/year}",ApplyFormatInEditMode =true)]
         [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}" ,ApplyFormatInEditMode = true )]

        public DateTime Date { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        [Display(Name = "Movie's Time")]
        public string Time { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Hall [A1,A2,A3,B1,B2]")]
        public string hall { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Price")]
        public float price { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Discout")]
        public float Discount { get; set; }
        public virtual ICollection<BookingTable> booking { get; set; }


    }
}