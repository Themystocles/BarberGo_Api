using Application.Interfaces;
using Domain.Entities;
using Persistence.Data;

public class AuthService : IAuthService
{
    private readonly DataContext _context;
    private readonly TokenService _tokenService;

    public AuthService(DataContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<string> AutenticarOuRegistrarUsuarioGoogleAsync(string email, string nome, string fotoPerfil)
    {
        var usuario = _context.AppUsers.FirstOrDefault(u => u.Email == email);

        if (usuario == null)
        {
            usuario = new AppUser
            {
                Name = nome,
                Email = email,
                Type = TipoUsuario.Client,
                ProfilePictureUrl = fotoPerfil
            };

            _context.AppUsers.Add(usuario);
            await _context.SaveChangesAsync();
        }
        else if (string.IsNullOrEmpty(usuario.ProfilePictureUrl))
        {
            usuario.ProfilePictureUrl = fotoPerfil;
            _context.AppUsers.Update(usuario);
            await _context.SaveChangesAsync();
        }

        return _tokenService.GenerateToken(usuario.Email, usuario.Type);
    }
}
