using Persistence.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Persistence.Repositories
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
        public async Task<AppUser?> GetUserByEmail(string email)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(u => u.Email == email);
        }


    }
}
