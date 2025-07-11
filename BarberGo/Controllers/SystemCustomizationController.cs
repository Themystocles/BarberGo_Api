using BarberGo.Entities;
using BarberGo.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberGo.Controllers
{
    public class SystemCustomizationController : GenericRepositoryController<SystemCustomization>
    {
        public SystemCustomizationController(GenericRepositoryServices<SystemCustomization> genericRepositoryServices)
             : base(genericRepositoryServices)
        {
          
        }



    }
}
