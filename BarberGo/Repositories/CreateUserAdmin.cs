using BarberGo.Data;
using BarberGo.Entities;
using BarberGo.Interfaces;

namespace BarberGo.Repositories
{
    public class CreateUserAdmin : ICreateUserAdmin
    {
        private readonly DataContext _dataContext;

        public CreateUserAdmin(DataContext dataContext)
        {
            _dataContext = dataContext;
            
        }
        public async Task<AppUser> CreateAdminAppUser(AppUser appUser)
        {
            await _dataContext.AddAsync(appUser);
            await _dataContext.SaveChangesAsync();

            return appUser;
        }
    }
}
