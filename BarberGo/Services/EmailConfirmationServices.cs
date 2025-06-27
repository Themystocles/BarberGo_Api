using BarberGo.Data;
using BarberGo.Entities;
using BarberGo.Entities.DTOs;
using BarberGo.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace BarberGo.Services
{
    public class EmailConfirmationServices
    {
        private readonly DataContext _dataContext;
        private readonly IEmailSender _emailSender;
        public EmailConfirmationServices(DataContext dataContext, IEmailSender emailSender)
        {
            _dataContext = dataContext;
            _emailSender = emailSender;
        }

        public async Task<string> SendCodeverifyEmail(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }
            var codigo = new Random().Next(100000, 999999).ToString();
            var validade = DateTime.UtcNow.AddMinutes(15);

            var emailveryfication = new EmailVerification
            {
                Email = email,
                Code = codigo,
                Expiration = validade,
                Verified = false
            };

            _dataContext.EmailVerification.Add(emailveryfication);

            await _emailSender.SendEmailAsync(email, "Código de recuperação", $"Seu código é: {codigo}");
            await _dataContext.SaveChangesAsync();

            return codigo;
        }
        public async Task<bool> VerificationEmail(string email, string code)
        {
            var verification = await _dataContext.EmailVerification.FirstOrDefaultAsync(x => x.Email == email);

            if (verification == null || verification.Expiration < DateTime.UtcNow || verification.Code != code)
                return false;

            verification.Verified = true;
            await _dataContext.SaveChangesAsync();


            return true;

        }
    }
}
