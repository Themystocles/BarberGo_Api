using BarberGo.Data;
using BarberGo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BarberGo.Repositories
{
    public class LoginUserRepository
    {
        private readonly DataContext _context;

        public LoginUserRepository(DataContext context) {
            _context = context; 
        }

        public async Task<AppUser?> GetUserByUsernameAsync(string username)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(x => x.Email == username);
        }
    }
}
