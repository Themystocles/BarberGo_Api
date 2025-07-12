using BarberGo.Entities;
using BarberGo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberGo.Controllers
{
    public class SystemCustomizationController : GenericRepositoryController<SystemCustomization>
    {
        public SystemCustomizationController(GenericRepositoryServices<SystemCustomization> genericRepositoryServices)
             : base(genericRepositoryServices)
        {
          
        }

        [AllowAnonymous]
        [HttpGet]
        public override async Task<ActionResult<List<SystemCustomization>>> GetAllEntities()
        {
            var entities = await _genericRepositoryServices.GetList();
            return Ok(entities);
        }
        [AllowAnonymous]
        [HttpGet("find/{id}")]
        public override async Task<ActionResult<SystemCustomization>> GetByIdAsync(int id)
        {
            var entityExist = await _genericRepositoryServices.GetByIdAsync(id);
            return Ok(entityExist);
        }

        [HttpPut("update/{id}")]
        public override async Task<ActionResult<SystemCustomization>> UpdateEntity(int id, SystemCustomization entity)
        {
            Console.WriteLine($"Recebido update para id {id} com nomeSistema={entity.NomeSistema}");
            

            return await base.UpdateEntity(id, entity);
        }



    }
}
