using GymApp.Models;
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
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        // VERİTABANI KURALLARINI BURADA AYARLIYORUZ
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1. DECIMAL HATASINI ÇÖZME (Price alanı için)
            builder.Entity<Service>()
                .Property(s => s.Price)
                .HasColumnType("decimal(18,2)"); // 18 basamak, virgülden sonra 2 hane (Para birimi standardı)

            // 2. CASCADE DELETE HATASINI ÇÖZME (Appointment için)
            // SQL Server'ın kafası karışmasın diye "Restrict" (Kısıtla) diyoruz.
            // Yani: "Bir Antrenör silinirse, randevusu otomatik silinmesin, hata versin veya biz yönetelim."

            builder.Entity<Appointment>()
                .HasOne(a => a.Gym)
                .WithMany()
                .HasForeignKey(a => a.GymId)
                .OnDelete(DeleteBehavior.Restrict); // Gym silinirse Randevuya dokunma (Otomatik silme)

            builder.Entity<Appointment>()
                .HasOne(a => a.Trainer)
                .WithMany()
                .HasForeignKey(a => a.TrainerId)
                .OnDelete(DeleteBehavior.Restrict); // Trainer silinirse Randevuya dokunma

            builder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany()
                .HasForeignKey(a => a.ServiceId)
                .OnDelete(DeleteBehavior.Restrict); // Service silinirse Randevuya dokunma
        }
    }
}