namespace GymApp.Models
{
    public class AiRequestModel
    {
        public int Age { get; set; }
        public double Height { get; set; } // cm
        public double Weight { get; set; } // kg
        public string Gender { get; set; } // Kadın, Erkek
        public string Goal { get; set; } // Kilo Ver, Kas Yap, Fit Kal
    }

    public class AiResponseModel
    {
        public string BmiResult { get; set; } // Vücut Kitle İndeksi Sonucu
        public string Advice { get; set; } // Tavsiye Metni
        public List<string> WorkoutPlan { get; set; } // Egzersiz Listesi
    }
}