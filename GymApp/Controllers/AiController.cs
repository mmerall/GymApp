using Microsoft.AspNetCore.Mvc;
using GymApp.Models;
using System.Text;
using Newtonsoft.Json;

namespace GymApp.Controllers
{
    public class AiController : Controller
    {
       
        private const string ApiKey = "AIzaSyAjvSG3DPtn6PSkvFXZt19fR0LXMVyFFZY";

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePlan([FromBody] AiRequestModel request)
        {

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

            
            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={ApiKey}";

            var promptData = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = $"Sen samimi bir spor hocasısın. {request.Age} yaşında, {request.Gender}, {request.Weight} kilo, {request.Height} boyunda, hedefi '{request.Goal}' olan biri için program hazırla. Cevap metnini HTML formatında verme, sadece düz yazı olarak madde madde, emojilerle süsleyerek ve Türkçe yaz. Çok uzun olmasın." } } }
                }
            };

            using (var client = new HttpClient())
            {
                try
                {
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(promptData), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, jsonContent);
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        dynamic result = JsonConvert.DeserializeObject(responseString);
                        string aiText = result.candidates[0].content.parts[0].text;

                        return Json(new
                        {
                            BmiResult = $"BMI: {bmi:F1} - {bmiStatus}",

                            Advice = "Senin için harika bir başlangıç programı hazırladım! 💪",

                            WorkoutPlan = new string[] { aiText }
                        });
                    }
                    else
                    {
                        return Json(new { Advice = "Hata oluştu, lütfen tekrar dene." });
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