using Api.Entities;
using Api.Entities.DTOs;
using Api.Interfaces;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
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

        [HttpGet("weeklySchedule/{barberId}")]
        public async Task<ActionResult<List<WeeklySchedule>>> GetAllEntities(int barberId)
        {
            

            var week = await _weeklySchedule.GetWeeklyScheduleByBarberId(barberId);
            return Ok(week);
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
           await _weeklySchedule.CreateNewSchedule(schedule);

           return Ok(schedule);
        }
        [HttpPut("updateOv/{id}")]
        public override async Task<ActionResult<WeeklySchedule>> UpdateEntity(int id, WeeklySchedule schedule)
        {
            schedule.Id = id;
            await _weeklySchedule.UpdateScheduleAsync(schedule);

            return Ok(schedule);
        }
        [HttpGet("barbers")]
        public async Task <ActionResult<List<BarberDto>>> GetUserForType()
        {

            var barbers = await _weeklySchedule.GetUserForType();

            return Ok(barbers); 
 

        }
    }
}
