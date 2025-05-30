using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BarberGo.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using BarberGo.Data;

[ApiController]
[Route("auth")]
public class AuthGoogleController : ControllerBase
{
    private readonly TokenService _tokenService;
    private readonly DataContext _context;

    public AuthGoogleController(TokenService tokenService, DataContext context)
    {
        _tokenService = tokenService;
        _context = context;
    }

    // Inicia o login via Google, redireciona para o Google com callback configurado
    [HttpGet("google-login")]
    public IActionResult GoogleLogin()
    {
        var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        // Ajuste a URL para a rota correta do seu controller
        var redirectUri = isDevelopment
            ? "https://localhost:7032/auth/signin-google"
            : "https://barbergo-api.onrender.com/auth/signin-google";

        var properties = new AuthenticationProperties { RedirectUri = redirectUri };

        // Inicia o desafio de autenticação via Google
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    // Callback que o Google chama após autenticação do usuário
    [HttpGet("signin-google")]
    public async Task<IActionResult> GoogleResponse()
    {
        // Tenta autenticar a identidade que o middleware do Google criou no cookie
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!result.Succeeded)
            return BadRequest("Falha na autenticação via Google.");

        var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
        var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var nome = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(email))
            return BadRequest("Email não fornecido pelo Google.");

        // Verifica se o usuário já existe no banco
        var usuario = _context.AppUsers.FirstOrDefault(u => u.Email == email);
        if (usuario == null)
        {
            usuario = new AppUser
            {
                Name = nome,
                Email = email,
                Type = TipoUsuario.Client // Ajuste conforme sua enumeração ou lógica de tipos
            };
            _context.AppUsers.Add(usuario);
            await _context.SaveChangesAsync();
        }

        // Gera token JWT para o usuário autenticado
        var token = _tokenService.GenerateToken(usuario.Email, usuario.Type);

        // Redireciona para o frontend com o token na query string
        var frontendUrl = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
            ? "http://localhost:5173/login-success"
            : "https://barbergo-ui.onrender.com/login-success";

        return Redirect($"{frontendUrl}?token={token}");
    }
}
