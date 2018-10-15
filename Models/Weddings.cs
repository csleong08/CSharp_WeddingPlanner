using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class Weddings
    {
        [Key]
        public int id { get; set; }
        public string wedder_one { get; set; }
        public string wedder_two { get; set; }
        public DateTime date { get; set; }
        public string address{ get; set; }
        public DateTime updated_at { get; set; }
        public DateTime created_at { get; set; }
        public int usersid { get; set; }
        public Users Users { get; set; }
        public Weddings()
        {
            Reservation = new List<Reservations>();
        }
        public List<Reservations> Reservation { get; set;}
    }
}