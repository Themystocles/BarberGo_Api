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



    }
}
