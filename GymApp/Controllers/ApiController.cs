using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymApp.Data;
using GymApp.Models;

namespace GymApp.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Tüm Salonları Getiren API
        // Adres: /api/gyms
        [HttpGet("gyms")]
        public async Task<ActionResult<IEnumerable<Gym>>> GetGyms()
        {
            return await _context.Gyms.ToListAsync();
        }

        // 2. Tüm Antrenörleri Getiren API
        // Adres: /api/trainers
        [HttpGet("trainers")]
        public async Task<ActionResult<IEnumerable<object>>> GetTrainers()
        {
            // Döngüye girmesin diye sadece gerekli alanları seçiyoruz (DTO mantığı)
            var trainers = await _context.Trainers
                .Select(t => new {
                    t.TrainerId,
                    t.FullName,
                    t.Specialization,
                    GymName = t.Gym.Name
                })
                .ToListAsync();

            return Ok(trainers);
        }

        // 3. Belirli Bir Salondaki Antrenörleri Getiren API (Filtreleme)
        // Adres: /api/trainers/5
        [HttpGet("trainers/{gymId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetTrainersByGym(int gymId)
        {
            var trainers = await _context.Trainers
                .Where(t => t.GymId == gymId) // Hata veren yer burasıydı, düzelttik.
                .Select(t => new {
                    t.TrainerId,
                    t.FullName,
                    t.Specialization
                })
                .ToListAsync();

            if (!trainers.Any())
            {
                return NotFound("Bu salonda henüz antrenör yok.");
            }

            return Ok(trainers);
        }

        // 4. Tüm Hizmetleri Getiren API
        // Adres: /api/services
        [HttpGet("services")]
        public async Task<ActionResult<IEnumerable<object>>> GetServices()
        {
            var services = await _context.Services
               .Select(s => new {
                   s.ServiceId,
                   s.Name,
                   s.Price,
                   s.Duration
               })
               .ToListAsync();

            return Ok(services);
        }
    }
}