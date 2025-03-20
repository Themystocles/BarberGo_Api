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
            var user = await _userRepository.GetUserByUsernameAsync(model.Username);

            if (user == null || user.PasswordHash != model.Password)
            {
                return null;
            }
            return _tokenService.GenerateToken(user.Name);
        }
    }
}
