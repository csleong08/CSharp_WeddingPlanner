using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class Reservations
    {
        [Key]
        public int id { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime created_at { get; set; }
        public int usersid { get; set; }
        public Users Users { get; set; }
        public int weddingsid { get; set; }
        public Weddings Weddings { get; set; }
    }
}