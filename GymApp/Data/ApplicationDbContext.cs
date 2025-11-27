using GymApp.Models; // Modeli tanıması için bunu ekledik
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        
        public DbSet<Gym> Gyms { get; set; }
        public DbSet<GymApp.Models.Trainer> Trainers { get; set; }
    }
}