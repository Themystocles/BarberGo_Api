using BarberGo.Entities;
using BarberGo.Interfaces;
using BarberGo.Repositories;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BarberGo.Services
{
    public class CreateUserAdminServices
    {
        private readonly ICreateUserAdmin _createUserAdmin;
        public CreateUserAdminServices(ICreateUserAdmin createUserAdmin)
        {
            _createUserAdmin = createUserAdmin;
        }
        public async Task <AppUser> CreateAppuserAdminAsync(AppUser appUser)
        {
            if (appUser == null)
            {
                throw new ArgumentNullException(nameof(appUser));
            }
            var createdUser = await _createUserAdmin.CreateAdminAppUser(appUser);
            return createdUser;
        }

    }
}
