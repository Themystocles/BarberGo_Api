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
       
    }
  
}
