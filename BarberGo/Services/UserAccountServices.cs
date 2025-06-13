using BarberGo.Entities;
using BarberGo.Interfaces;
using BarberGo.Repositories;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity;

namespace BarberGo.Services
{
    public class UserAccountServices
    {
        private readonly IUserAccountRepository _userAccountRepositoty;
        public UserAccountServices(IUserAccountRepository createUserAdmin)
        {
            _userAccountRepositoty = createUserAdmin;
        }
        public async Task <AppUser> CreateAppuserAdminAsync(AppUser appUser)
        {
            if (appUser == null)
            {
                throw new ArgumentNullException(nameof(appUser));
            }
            var createdUser = await _userAccountRepositoty.CreateAdminAppUser(appUser);
            return createdUser;
        }

        public async Task VerifyEmailExsist(string Email)
        {
            if (string.IsNullOrEmpty(Email)) 
            {
                throw new ArgumentNullException(nameof(Email), "O email não pode ser nulo ou vazio.");
            }
            var emailExists = await _userAccountRepositoty.EmailExistsAsync(Email);
            if (emailExists)
            {
                throw new InvalidOperationException("Já existe um usuário com esse email.");
            }
        }
        public async Task <bool> verifyPassword(AppUser entity, string password)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            var passwordHasher = new PasswordHasher<AppUser>();

            var result = passwordHasher.VerifyHashedPassword(entity, entity.PasswordHash, password);

            return result == PasswordVerificationResult.Success;



        }

    }
}
