using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Models
{
    public class WeddingPlannerContext : DbContext
    {
        public DbSet<Users> users { get; set; } // always make users lowercase
        public DbSet<Weddings> weddings { get; set; } // always make users lowercase
        public DbSet<Reservations> reservations { get; set; } // always make users lowercase

        // base() calls the parent class' constructor passing the "options" parameter along
        public WeddingPlannerContext(DbContextOptions<WeddingPlannerContext> options) : base(options) { }
    }
}