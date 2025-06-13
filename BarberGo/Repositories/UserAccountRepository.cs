using BarberGo.Data;
using BarberGo.Entities;
using BarberGo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BarberGo.Repositories
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
    }
}
