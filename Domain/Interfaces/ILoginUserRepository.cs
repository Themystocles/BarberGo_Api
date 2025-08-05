using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ILoginUserRepository
    {
        Task<AppUser?> GetUserByUsernameAsync(string username);
        Task<AppUser?> GetUserByEmail(string email);
    }
}
