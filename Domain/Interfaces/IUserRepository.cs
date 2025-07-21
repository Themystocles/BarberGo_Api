using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserByEmailAsync(string email);
        Task CreateUserAsync(AppUser user);
    }

}
