using Api.Data;
using Api.Entities;
using Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly DataContext _dataContext;

        public UserAccountRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
            
        }
        public async Task<AppUser> CreateAdminAppUser(AppUser appUser)
        {
            await _dataContext.AddAsync(appUser);
            await _dataContext.SaveChangesAsync();

            return appUser;
        }

        public async Task<bool> EmailExistsAsync(string Email)
        {
          var user =  await _dataContext.AppUsers.AsNoTracking().FirstOrDefaultAsync(u => u.Email == Email);
          return user != null;
            
        }

        public async Task<AppUser> GetUserByEmail(string email)
        {
            var user = await _dataContext.AppUsers.FirstOrDefaultAsync(e => e.Email == email);

            

            return user;


        }

        public async Task<AppUser> ToggleAdminStatus(AppUser appUser)
        {
             _dataContext.Update(appUser);

            await _dataContext.SaveChangesAsync();

            return appUser;
        }
    }
}
