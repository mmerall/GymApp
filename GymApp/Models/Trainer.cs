using System.ComponentModel.DataAnnotations;

namespace GymApp.Models
{
    public class Trainer
    {
        [Key]
        public int TrainerId { get; set; }

        [Required(ErrorMessage = "Ad Soyad girmelisiniz.")]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Uzmanlık alanı belirtilmelidir.")]
        [Display(Name = "Uzmanlık Alanı")]
        public string Specialization { get; set; } // Örn: Fitness, Pilates, Yoga

        [Display(Name = "Profil Resmi (URL)")]
        public string? ImageUrl { get; set; } // Antrenörün fotoğrafı

        [Range(0, 50, ErrorMessage = "Deneyim 0-50 yıl arasında olmalıdır.")]
        [Display(Name = "Deneyim (Yıl)")]
        public int Experience { get; set; }

        // --- HANGİ SALONDA ÇALIŞIYOR? ---
        [Display(Name = "Çalıştığı Salon")]
        public int GymId { get; set; } // İlişki anahtarı

        public Gym? Gym { get; set; } // Bağlantı
    }
}