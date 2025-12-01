using Microsoft.AspNetCore.Mvc;
using GymApp.Models;

namespace GymApp.Controllers
{
    public class AiController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GeneratePlan([FromBody] AiRequestModel request)
        {
            // 1. Vücut Kitle İndeksi (BMI) Hesapla
            // Formül: Kilo / (Boy * Boy) [Metre cinsinden]
            double heightInMeters = request.Height / 100;
            double bmi = request.Weight / (heightInMeters * heightInMeters);

            var response = new AiResponseModel();
            response.WorkoutPlan = new List<string>();

            // 2. BMI Durumu Belirle
            string bmiStatus = "";
            if (bmi < 18.5) bmiStatus = "Zayıf";
            else if (bmi < 25) bmiStatus = "Normal";
            else if (bmi < 30) bmiStatus = "Fazla Kilolu";
            else bmiStatus = "Obez";

            response.BmiResult = $"Vücut Kitle İndeksin: {bmi:F1} ({bmiStatus})";

            // 3. Hedefe ve Duruma Göre Akıllı Program Oluştur (Sanki AI yazmış gibi)

            // SENARYO A: KİLO VERMEK İSTİYORSA
            if (request.Goal == "Kilo Ver")
            {
                response.Advice = $"Merhaba! {request.Age} yaşında bir {request.Gender} olarak, şu anki vücut analizin '{bmiStatus}' kategorisinde. Senin için metabolizmanı hızlandıracak 'High Intensity' (HIIT) odaklı bir program hazırladım. Önceliğimiz kalori açığı oluşturmak.";

                response.WorkoutPlan.Add("🏃‍♂️ 20 Dk Sabah Aç Karnına Kardiyo (Yürüyüş/Koşu)");
                response.WorkoutPlan.Add("🔥 3x15 Burpees & Jumping Jacks (Süper Set)");
                response.WorkoutPlan.Add("🏋️‍♂️ Tüm Vücut Dambıl Antrenmanı (Hafif Kilo, Çok Tekrar)");
                response.WorkoutPlan.Add("🥗 Diyet Önerisi: Karbonhidratı azalt, protein ve sebze ağırlıklı beslen.");
            }
            // SENARYO B: KAS YAPMAK İSTİYORSA
            else if (request.Goal == "Kas Yap")
            {
                response.Advice = $"Harika bir hedef! {bmi:F1} BMI değerin kas inşası için uygun bir temel sağlıyor. Senin için hipertrofi (kas büyümesi) odaklı, 'Progressive Overload' prensibine dayalı bir program hazırladım.";

                response.WorkoutPlan.Add("💪 4x8 Bench Press (Göğüs & Arka Kol)");
                response.WorkoutPlan.Add("🏋️‍♂️ 4x10 Squat & Deadlift (Bacak & Sırt)");
                response.WorkoutPlan.Add("🧴 Kreatin ve Whey Protein takviyesi düşünülebilir.");
                response.WorkoutPlan.Add("🥩 Diyet Önerisi: Günlük kilon başına x2 gr protein almalısın.");
            }
            // SENARYO C: FİT KALMAK İSTİYORSA
            else
            {
                response.Advice = $"Formunu korumak istiyorsun, bu harika! {bmiStatus} bir vücut yapın var. Senin için dayanıklılığını ve esnekliğini artıracak hibrit bir program hazırladım.";

                response.WorkoutPlan.Add("🧘‍♀️ 30 Dk Yoga & Pilates (Esneklik)");
                response.WorkoutPlan.Add("🏊‍♂️ Haftada 2 gün Yüzme veya Bisiklet");
                response.WorkoutPlan.Add("🤸‍♂️ Fonksiyonel Kuvvet Antrenmanı (Kendi vücut ağırlığınla)");
            }

            // JSON Olarak Geri Dön (Sayfa yenilenmeden göstermek için)
            return Json(response);
        }
    }
} 