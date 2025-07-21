using Domain.Entities;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    public class HaircutsController : GenericRepositoryController<Haircut>
    {
        public HaircutsController(GenericRepositoryServices<Haircut> genericRepositoryServices)
                : base(genericRepositoryServices)
        {
           
        }
        [AllowAnonymous]
        [HttpGet]
        public override async Task<ActionResult<List<Haircut>>> GetAllEntities()
        {
            var entities = await _genericRepositoryServices.GetList();
            return Ok(entities);
        }

        [HttpPut("update/{id}")]
        public override async Task<ActionResult<Haircut>> UpdateEntity(int id, Haircut entity)
        {
            entity.Id = id;
            await _genericRepositoryServices.UpdateAsync(entity);
            return Ok(entity);
        }

        [HttpDelete("delete/{id}")]
        public override async Task<ActionResult<bool>> DeleteEntity(int id)
        {
            bool deleted = await _genericRepositoryServices.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();

        }

        [HttpPost("create")]
        public override async Task<ActionResult<Haircut>> CreateEntity(Haircut entity)
        {
            var createdEntity = await _genericRepositoryServices.CreateAsync(entity);
            return Created("", createdEntity);

        }


    }
}
