using BarberGo.Entities;
using BarberGo.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberGo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HaircutsController : GenericRepositoryController<Haircut>
    {
        public HaircutsController(GenericRepositoryServices<Haircut> genericRepositoryServices)
                : base(genericRepositoryServices)
        {
           
        }



    }
}
