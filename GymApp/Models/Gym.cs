using NuGet.DependencyResolver;
using System.ComponentModel.DataAnnotations;

namespace GymApp.Models
{
    public class Gym
    {
        [Key]
        public int GymId { get; set; }

        [Required(ErrorMessage = "Salon adı boş bırakılamaz.")]
        [StringLength(100)]
        [Display(Name = "Salon Adı")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Adres boş bırakılamaz.")]
        [StringLength(200)]
        [Display(Name = "Adres")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Çalışma saatleri belirtilmelidir.")]
        [Display(Name = "Çalışma Saatleri")]
        public string OpeningHours { get; set; }

        [Display(Name = "Salon Resmi (URL)")]
        public string? ImageUrl { get; set; }

        [Range(0, 5, ErrorMessage = "Puan 0-5 arası olmalı.")]
        public double Rating { get; set; } = 0.0;

        // --- YENİ EKLEYECEĞİMİZ ALANLAR ---

        [Display(Name = "Salon Açıklaması")]
        public string? Description { get; set; } // Uzun açıklama yazısı

        [Display(Name = "WiFi Var mı?")]
        public bool HasWifi { get; set; } // Checkbox olacak

        [Display(Name = "Duş Var mı?")]
        public bool HasShowers { get; set; } // Checkbox olacak

        [Display(Name = "Müzik Yayını Var mı?")]
        public bool HasMusic { get; set; } // Checkbox olacak

        // ----------------------------------

        public ICollection<Trainer>? Trainers { get; set; }
    }
}