using Api.Data;
using Api.Entities;
using Api.Interfaces;
using Api.Interfaces.Api.Interfaces;
using Api.Services;
using Microsoft.AspNetCore.Identity;

namespace Api.Repositories
{
    public class RecoveryPasswordRepository : IRecoveryPassword
    {
        private readonly IUserAccountRepository _userRepository;
        private readonly DataContext _dataContext;
        private readonly IEmailSender _emailSender;
        private readonly IPasswordHasher<AppUser> _passwordHasher;

        public RecoveryPasswordRepository(IUserAccountRepository userAccountRepositor, DataContext context, IEmailSender emailSender, IPasswordHasher<AppUser> passwordHasher)
        {
            _userRepository = userAccountRepositor;
            _dataContext = context;
            _emailSender = emailSender;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> ResetPasswordWithCodeAsync(string email, string code, string newPassword)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null) return false;
            if (string.IsNullOrWhiteSpace(newPassword)) return false;

            if (user.RecoveryCode == code && user.RecoveryCodeExpiration >= DateTime.UtcNow)
            {
               
                user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);

                
                user.RecoveryCode = null;
                user.RecoveryCodeExpiration = null;

                _dataContext.Update(user);
                await _dataContext.SaveChangesAsync();

                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SendRecoveryCodeAsync(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null) return false;

            var codigo = new Random().Next(100000, 999999).ToString();
            var validade = DateTime.UtcNow.AddMinutes(15);

            user.RecoveryCode = codigo;
            user.RecoveryCodeExpiration = validade;

             _dataContext.Update(user); 
            await _dataContext.SaveChangesAsync();

            await _emailSender.SendEmailAsync(email, "Código de recuperação", $"Seu código é: {codigo}");

            return true;



        }
    }
}
