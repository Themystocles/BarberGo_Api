using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserAccountRepository
    {
        public Task<AppUser> CreateAdminAppUser(AppUser appUser);
        public Task<bool> EmailExistsAsync(string Email);

        public Task<AppUser> ToggleAdminStatus(AppUser appUser);

        public Task<AppUser> GetUserByEmail(string email);
    }
}
