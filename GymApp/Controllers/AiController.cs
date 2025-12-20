using Microsoft.AspNetCore.Mvc;
using GymApp.Models;
using System.Text;
using Newtonsoft.Json;

namespace GymApp.Controllers
{
    public class AiController : Controller
    {
        // 👇 ÇALIŞAN KEY'İN
        private const string ApiKey = "...";

        // 👇 ÇALIŞAN SABİT ADRES
        private const string BaseApiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-flash-latest:generateContent";

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePlan([FromBody] AiRequestModel request)
        {
            // 1. BMI Hesaplama
            double heightInMeters = request.Height / 100.0;
            double bmi = 0;
            string bmiStatus = "";

            if (heightInMeters > 0)
            {
                bmi = request.Weight / (heightInMeters * heightInMeters);
                if (bmi < 18.5) bmiStatus = "Zayıf";
                else if (bmi < 25) bmiStatus = "Normal Kilo";
                else if (bmi < 30) bmiStatus = "Fazla Kilo";
                else bmiStatus = "Obezite";
            }

            // ⭐ GELİŞMİŞ FOTOĞRAF MANTIĞI (Yaş + Cinsiyet + Hedef)
            // Mantık: "{cinsiyet}-{yasgrubu}-{hedef}.jpg" ismini oluşturacağız.

            // A) Cinsiyeti Belirle
            string genderPart = "erkek"; // Varsayılan
            if (request.Gender == "Kadın" || request.Gender == "Kadin") genderPart = "kadin";

            // B) Yaş Grubunu Belirle (35 yaş sınırı)
            string agePart = "genc"; // Varsayılan (35 altı)
            if (request.Age >= 35) agePart = "yasli";

            // C) Hedefi Belirle
            string goalPart = "fit"; // Varsayılan
            if (request.Goal == "Kilo Vermek") goalPart = "zayiflama";
            else if (request.Goal == "Kas Yapmak" || request.Goal == "Karın Kası Yapmak") goalPart = "kas";

            // D) Parçaları Birleştir: Örn: "/images/hedefler/erkek-yasli-kas.jpg"
            string targetImagePath = $"/images/hedefler/{genderPart}-{agePart}-{goalPart}.jpg";
            // -----------------------------------------------------------

            // 3. Prompt (HTML)
            string promptText = $"Sen samimi bir spor hocasısın. {request.Age} yaşında, {request.Gender}, {request.Weight} kilo, {request.Height} boyunda, hedefi '{request.Goal}' olan biri için program hazırla. Cevabı süslü bir HTML formatında ver. Başlıkları <h3> ile, kalın yerleri <strong> ile, listeleri <ul> ve <li> ile yap. Asla ```html yazma, direkt kodları ver.";

            var promptData = new
            {
                contents = new[] { new { parts = new[] { new { text = promptText } } } }
            };

            using (var client = new HttpClient())
            {
                try
                {
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(promptData), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync($"{BaseApiUrl}?key={ApiKey}", jsonContent);
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        dynamic result = JsonConvert.DeserializeObject(responseString);

                        try
                        {
                            string aiText = result.candidates[0].content.parts[0].text;
                            aiText = aiText.Replace("```html", "").Replace("```", "").Trim();

                            return Json(new
                            {
                                BmiResult = $"BMI: {bmi:F1} - {bmiStatus}",
                                Advice = "Programın hazır! İşte sana özel plan: 👇",
                                WorkoutPlan = new string[] { aiText },
                                TargetImage = targetImagePath
                            });
                        }
                        catch
                        {
                            return Json(new { Advice = "Yapay zeka cevap veremedi." });
                        }
                    }
                    else
                    {
                        return Json(new { Advice = "Google Hatası: " + response.StatusCode });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { Advice = "Sistem Hatası: " + ex.Message });
                }
            }
        }
    }
}
