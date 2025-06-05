using BarberGo.Entities;

namespace BarberGo.Interfaces
{
    public interface ICreateUserAdmin
    {
        public Task<AppUser> CreateAdminAppUser(AppUser appUser);
    }
}
