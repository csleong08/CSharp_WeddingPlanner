using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class UsersValidate
    {
        [Required]
        [MinLength(2)]
        [RegularExpression("^([a-zA-Z])+$", ErrorMessage = "Names cannot be numbers")]
        public string first_name { get; set; }
        [Required]
        [MinLength(2)]
        [RegularExpression("^([a-zA-Z])+$", ErrorMessage = "Names cannot be numbers")]
        public string last_name { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Passwords must be at least 8 characters")]
        public string password { get; set; }

        [Display(Name = "Confirm password")]
        [Compare("password", ErrorMessage = "The passwords do not match.")]
        public string passconfirm { get; set; }
    }
}