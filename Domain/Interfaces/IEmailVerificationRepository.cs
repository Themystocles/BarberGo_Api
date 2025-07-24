using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IEmailVerificationRepository
    {
        Task<EmailVerification?> GetByEmailAsync(string email);
        Task AddAsync(EmailVerification verification);
        Task UpdateAsync(EmailVerification verification);
        Task SaveChangesAsync();
    }
}
