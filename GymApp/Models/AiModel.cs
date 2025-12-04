namespace GymApp.Models
{
    // Kullanıcıdan gelen veriler
    public class AiRequestModel
    {
        public int Age { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public string Gender { get; set; }
        public string Goal { get; set; }
    }

    // Yapay Zekadan alıp Ekrana basacağımız cevap formatı
    public class AiResponseModel
    {
        public string BmiResult { get; set; }
        public string Advice { get; set; }
        public List<string> WorkoutPlan { get; set; }
    }
}