using BarberGo.Entities;
using BarberGo.Interfaces;
using BarberGo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarberGo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeeklyScheduleController : GenericRepositoryController<WeeklySchedule>
    {
        private readonly IWeeklySchedule _weeklySchedule;
        public WeeklyScheduleController(GenericRepositoryServices<WeeklySchedule> genericRepositoryServices, IWeeklySchedule weeklySchedule)
             : base(genericRepositoryServices)
        {
            _weeklySchedule = weeklySchedule;
        }
        [HttpGet("available-slots")]
        public async Task<IActionResult> GetAvailableSlots([FromQuery] DateTime date, [FromQuery] int? barberId)
        {
            var result = await _weeklySchedule.GetAvailableSlotsAsync(date, barberId);
            return Ok(result);
        }
        [Authorize]
        [HttpPost("create-weekly")]
        public override async Task<ActionResult<WeeklySchedule>> CreateEntity(WeeklySchedule schedule)
        {
            _weeklySchedule.CreateNewSchedule(schedule);

           return Ok(schedule);
        }
    }
}
