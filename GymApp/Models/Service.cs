using System.ComponentModel.DataAnnotations;

namespace GymApp.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Hizmet adı girmelisiniz.")]
        [Display(Name = "Hizmet Adı")]
        public string Name { get; set; } // Örn: Pilates, Yoga, Boks

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Süre belirtilmelidir.")]
        [Range(10, 180, ErrorMessage = "Süre 10-180 dk arasında olmalı.")]
        [Display(Name = "Süre (Dakika)")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Ücret belirtilmelidir.")]
        [Range(0, 10000, ErrorMessage = "Geçerli bir tutar giriniz.")]
        [Display(Name = "Ücret (₺)")]
        public decimal Price { get; set; }

        // --- HANGİ SALONDA BU HİZMET VAR? ---
        [Display(Name = "Salon")]
        public int GymId { get; set; }

        public Gym? Gym { get; set; }
    }
}