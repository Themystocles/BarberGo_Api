using BarberGo.Entities;
using BarberGo.Repositories;

namespace BarberGo.Services
{
    public class LoginServices
    {
        private readonly TokenService _tokenService;
        private readonly LoginUserRepository _userRepository;

        public LoginServices(TokenService tokenService, LoginUserRepository userRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }
        public async Task<string> LoginAppUser(LoginModel model)
        {
            var user = await _userRepository.GetUserByUsernameAsync(model.Email);

            if (user == null || user.PasswordHash != model.Password)
            {
                return null;
            }
            return _tokenService.GenerateToken(user.Email, user.Type);
        }
        public async Task<AppUser?> GetUserProfileByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email inválido.");

            var user = await _userRepository.GetUserByEmail(email);

            return user;
        }
    }
}
