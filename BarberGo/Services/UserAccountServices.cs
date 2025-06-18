using BarberGo.Entities;
using BarberGo.Interfaces;
using BarberGo.Repositories;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity;
using BarberGo.Entities.DTOs;
using System.Security;

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
        public async Task <bool> verifyPassword(string password, string confirmpassword)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            if (confirmpassword == null)
                throw new ArgumentNullException(nameof(confirmpassword));
            if (password != confirmpassword)
                throw new InvalidOperationException("A senha e a confirmação de senha devem ser iguais.");


            return true;



        }
        public async Task<AppUser> UpdateUserToAdmin(string email)
        {
            if(string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email), "O email não pode ser vazio ou nulo.");
            }

            var user = await _userAccountRepositoty.GetUserByEmail(email);

            if (user == null)
            {
                throw new InvalidOperationException("O Email informado está incorreto ou não existe");
            }


            user.Type = TipoUsuario.Administrator;



            await _userAccountRepositoty.UpdateUserToAdmin(user);

            return user;



           

            
        }

    }
}
