
using BarberGo.Interfaces;
using BarberGo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BarberGo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenericRepositoryController<T> : ControllerBase where T : class, IEntity
    {
        public readonly GenericRepositoryServices<T> _genericRepositoryServices;

        public GenericRepositoryController(GenericRepositoryServices<T> genericRepositoryServices)
        {
            _genericRepositoryServices = genericRepositoryServices;
        }
        [HttpPost("create")]
        public async Task<ActionResult<T>> CreateEntity(T entity)
        {
            await _genericRepositoryServices.CreateAsync(entity);
            return Ok(entity);
        }
        [HttpPut("update")]
        public async Task<ActionResult<T>> UpdateEntity(T entity)
        {
            return Ok(entity);
        }
    }
}
