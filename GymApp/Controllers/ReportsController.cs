using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Controllers
{
    [Authorize(Roles = "Admin")] // Sadece yöneticiler raporları görebilir
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}