using BarberGo.Entities;
using BarberGo.Interfaces;
using BarberGo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BarberGo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MyCustomersController : ControllerBase
    {
        public readonly ITodaysCustomers _todaysCustomers;

        public MyCustomersController(ITodaysCustomers todaysCustomers)
        {
            _todaysCustomers = todaysCustomers;
        }

        [HttpGet("ClientesDoDia/{barberid}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetCustomersOfTheDay(int barberid, [FromQuery] DateTime date)
        {

            var entities = await _todaysCustomers.MyCustomersOfTheDay(date, barberid);
            return Ok(entities);
        }
    }
}
