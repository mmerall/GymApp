using GymApp.Data;
using GymApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GymApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Veritabanýndan gerçek sayýlarý çekiyoruz
            // Eðer veritabaný boþsa hata vermesin diye 0 atýyoruz

            ViewBag.SalonSayisi = _context.Gyms.Count();
            ViewBag.EgitmenSayisi = _context.Trainers.Count();
            ViewBag.UyeSayisi = _context.Users.Count();

            // Ciro hesaplama: (Hizmet Fiyatý x O hizmete alýnan randevu sayýsý)
            // Basitlik olsun diye Hizmet Fiyatlarýnýn toplamýný alabiliriz veya rastgele bir mantýk kurabiliriz.
            // Burada: (Toplam Hizmet Sayýsý * 500 TL) gibi basit bir demo yapalým.
            ViewBag.TahminiCiro = _context.Appointments.Count() * 250;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}