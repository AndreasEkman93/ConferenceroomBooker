using ConferenceroomBooker.Models;
using Microsoft.EntityFrameworkCore;

namespace ConferenceroomBooker.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
            
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ConferenceRoom> ConferenceRooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
