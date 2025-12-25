using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TrainsController(AppDbContext context)
        {
            _context = context;
        }

        // 1. ПОЛУЧИТЬ ВСЕ ПОЕЗДА (Для клиента)
       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Train>>> GetTrains()
        {
            return await _context.Trains
                .Include(t => t.Route) // Подгружаем маршрут
                .ThenInclude(rs => rs.Station) // Подгружаем названия станций
                .ToListAsync();
        }

        // 2. ОБНОВИТЬ ПОЛОЖЕНИЕ ПОЕЗДА (Для датчика из Wokwi)
        // Когда поезд проезжает станцию, датчик вызывает этот метод
        [HttpPost("{trainId}/update-location/{stationId}")]
        public async Task<IActionResult> UpdateLocation(int trainId, int stationId, string actualTime)
        {
            var train = await _context.Trains.FindAsync(trainId);
            if (train == null) return NotFound("Поїзд не знайден");

            // Находим в графике эту станцию для этого поезда
            var schedule = await _context.RouteStops
                .FirstOrDefaultAsync(rs => rs.TrainId == trainId && rs.StationId == stationId);

            if (schedule != null)
            {
                //задержка. если приехал позже, чем в ScheduledArrival — считаем разницу
                train.CurrentStationId = stationId;

                train.DelayMinutes = 5;
            }

            await _context.SaveChangesAsync();
            return Ok($"Поїзд {train.Number} тепер на станції {stationId}");
        }

        // 3. История
        [HttpPost("view/{userId}/{trainId}")]
        public async Task<IActionResult> RecordView(int userId, int trainId)
        {
            var history = new ViewHistory { UserId = userId, TrainId = trainId };
            _context.ViewHistories.Add(history);
            await _context.SaveChangesAsync();
            return Ok("Просмотр записан у історію");
        }

    }
}