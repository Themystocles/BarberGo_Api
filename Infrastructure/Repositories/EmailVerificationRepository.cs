// Infrastructure/Repositories/EmailVerificationRepository.cs
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

public class EmailVerificationRepository : IEmailVerificationRepository
{
    private readonly DataContext _context;

    public EmailVerificationRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<EmailVerification?> GetByEmailAsync(string email)
    {
        return await _context.EmailVerification.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task AddAsync(EmailVerification verification)
    {
        await _context.EmailVerification.AddAsync(verification);
    }

    public Task UpdateAsync(EmailVerification verification)
    {
        _context.EmailVerification.Update(verification);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
