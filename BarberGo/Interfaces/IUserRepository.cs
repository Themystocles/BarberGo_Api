using BarberGo.Entities;

namespace BarberGo.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserByEmailAsync(string email);
        Task CreateUserAsync(AppUser user);
    }

}
