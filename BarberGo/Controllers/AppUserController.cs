using BarberGo.Entities;
using BarberGo.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberGo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppUserController : GenericRepositoryController<AppUser>
    {
        public AppUserController(GenericRepositoryServices<AppUser> genericRepositoryServices)
              : base(genericRepositoryServices)
        {
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public override Task<ActionResult<List<AppUser>>> GetAllEntities()
        {
            return Task.FromResult<ActionResult<List<AppUser>>>(Forbid());
        }
      //  [ApiExplorerSettings(IgnoreApi = true)]
        public override Task<ActionResult<AppUser>> GetByIdAsync(int id)
        {
            return Task.FromResult<ActionResult<AppUser>>(Unauthorized("Acesso negado. Você não tem permissão para executar esta ação."));
        }

    }
  
}
