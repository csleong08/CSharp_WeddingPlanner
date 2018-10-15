using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class WeddingsValidate
    {
        [Required(ErrorMessage = "If you are Name-less, type in Name for your first name.")]
        [MinLength(2)]
        [RegularExpression("^([a-zA-Z])+$", ErrorMessage = "Names cannot be numbers")]
        public string wedder_one { get; set; }
        [Required(ErrorMessage = "If you are Name-less, type in Less for your last name.")]
        [MinLength(2)]
        [RegularExpression("^([a-zA-Z])+$", ErrorMessage = "Names cannot be numbers")]
        public string wedder_two { get; set; }
        [Required(ErrorMessage = "A date for the wedding is required")]
        public DateTime date { get; set; }
        [Required(ErrorMessage = "An address for the wedding is required. You can't hold a wedding in your heart alone.")]
        public string address { get; set; }
    }
}