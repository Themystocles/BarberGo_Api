using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly DbContext _context;  
    public UserRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<AppUser> GetUserByEmailAsync(string email)
    {
        return await _context.Set<AppUser>().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task CreateUserAsync(AppUser user)
    {
        _context.Set<AppUser>().Add(user);
        await _context.SaveChangesAsync();
    }
}
