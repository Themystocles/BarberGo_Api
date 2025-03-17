
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
           var createdEntity =  await _genericRepositoryServices.CreateAsync(entity);
            return Created("", createdEntity);

        }
        [HttpPut("update/{id}")]
        public async Task<ActionResult<T>> UpdateEntity(int id, T entity)
        {
            entity.Id = id;
           await _genericRepositoryServices.UpdateAsync(entity);
            return Ok(entity);
        }
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<bool>> DeleteEntity(int id)
        {
            bool deleted = await _genericRepositoryServices.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();

        }
    }
}
