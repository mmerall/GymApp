using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymApp.Data;
using GymApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GymApp.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. LİSTELEME (INDEX)
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var randevular = _context.Appointments
                .Include(a => a.Gym)
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .AsQueryable();

            // Eğer Admin değilse sadece kendi randevularını görsün
            if (!User.IsInRole("Admin"))
            {
                randevular = randevular.Where(a => a.UserId == userId);
            }

            return View(await randevular.OrderByDescending(a => a.AppointmentDate).ToListAsync());
        }

        // 2. DETAYLAR (DETAILS)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Gym)
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // 3. YENİ KAYIT SAYFASI (CREATE GET)
        public IActionResult Create()
        {
            ViewData["GymId"] = new SelectList(_context.Gyms, "GymId", "Name");
            // Diğer kutular boş gelsin, AJAX ile dolacak
            ViewData["TrainerId"] = new SelectList(new List<Trainer>(), "TrainerId", "FullName");
            ViewData["ServiceId"] = new SelectList(new List<Service>(), "ServiceId", "Name");
            return View();
        }

        // 4. YENİ KAYIT İŞLEMİ (CREATE POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentId,AppointmentDate,GymId,TrainerId,ServiceId")] Appointment appointment)
        {
            appointment.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            appointment.Status = "Onay Bekliyor";
            appointment.CreatedDate = DateTime.Now;

            // Çakışma Kontrolü
            bool cakismaVar = _context.Appointments.Any(a =>
                a.TrainerId == appointment.TrainerId &&
                a.AppointmentDate == appointment.AppointmentDate &&
                a.Status != "İptal");

            if (cakismaVar)
            {
                ModelState.AddModelError("AppointmentDate", "Seçtiğiniz saatte bu eğitmen dolu.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["GymId"] = new SelectList(_context.Gyms, "GymId", "Name", appointment.GymId);
            return View(appointment);
        }

        // 5. DÜZENLEME SAYFASI (EDIT GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            // Dropdownları doldur
            ViewData["GymId"] = new SelectList(_context.Gyms, "GymId", "Name", appointment.GymId);
            ViewData["TrainerId"] = new SelectList(_context.Trainers.Where(t => t.GymId == appointment.GymId), "TrainerId", "FullName", appointment.TrainerId);
            ViewData["ServiceId"] = new SelectList(_context.Services.Where(s => s.GymId == appointment.GymId), "ServiceId", "Name", appointment.ServiceId);

            return View(appointment);
        }

        // 6. DÜZENLEME İŞLEMİ (EDIT POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentId,AppointmentDate,GymId,TrainerId,ServiceId,Status")] Appointment appointment)
        {
            if (id != appointment.AppointmentId) return NotFound();

            // User ID'yi kaybetmemek için veritabanından eski kaydı bulup ID'sini alabiliriz
            // Ama şimdilik basit tutalım, Status güncelleniyorsa Admin'dir.

            // Eğer Admin değilse Status değişmesin diye kontrol eklenebilir ama View tarafında gizledik zaten.

            if (ModelState.IsValid)
            {
                try
                {
                    // EF Core bazen ilişkili verileri null sanabilir, o yüzden sadece değişenleri güncelleyelim
                    var mevcutKayit = await _context.Appointments.AsNoTracking().FirstOrDefaultAsync(x => x.AppointmentId == id);

                    if (mevcutKayit != null)
                    {
                        appointment.UserId = mevcutKayit.UserId; // Eski kullanıcının ID'sini koru
                        appointment.CreatedDate = mevcutKayit.CreatedDate; // Oluşturma tarihini koru

                        // Eğer Admin değilse eski statüyü koru
                        if (!User.IsInRole("Admin"))
                        {
                            appointment.Status = mevcutKayit.Status;
                        }
                    }

                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.AppointmentId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(appointment);
        }

        // 7. SİLME SAYFASI (DELETE GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Gym)
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // 8. SİLME İŞLEMİ (DELETE POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // --- AJAX METOTLARI ---
        [HttpGet]
        public JsonResult GetTrainersByGym(int gymId)
        {
            var trainers = _context.Trainers
                .Where(t => t.GymId == gymId)
                .Select(t => new { value = t.TrainerId, text = t.FullName })
                .ToList();
            return Json(trainers);
        }

        [HttpGet]
        public JsonResult GetServicesByGym(int gymId)
        {
            var services = _context.Services
                .Where(s => s.GymId == gymId)
                .Select(s => new { value = s.ServiceId, text = s.Name })
                .ToList();
            return Json(services);
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentId == id);
        }
    }
}