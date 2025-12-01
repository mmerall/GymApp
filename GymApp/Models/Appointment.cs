using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity; // Kullanıcıyı tanımak için gerekli

namespace GymApp.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Lütfen bir tarih ve saat seçiniz.")]
        [Display(Name = "Randevu Tarihi")]
        public DateTime AppointmentDate { get; set; }

        [Display(Name = "Durum")]
        public string Status { get; set; } = "Bekliyor"; // Varsayılan: Bekliyor, Onaylandı, İptal

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // --- İLİŞKİLER ---

        // 1. Randevuyu alan Üye (IdentityUser ile bağlıyoruz)
        [Display(Name = "Üye")]
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }

        // 2. Hangi Salon?
        [Display(Name = "Salon")]
        public int GymId { get; set; }
        public Gym? Gym { get; set; }

        // 3. Hangi Eğitmen?
        [Display(Name = "Eğitmen")]
        public int TrainerId { get; set; }
        public Trainer? Trainer { get; set; }

        // 4. Hangi Hizmet?
        [Display(Name = "Hizmet")]
        public int ServiceId { get; set; }
        public Service? Service { get; set; }
    }
}