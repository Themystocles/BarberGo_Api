using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BarberGo.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using BarberGo.Data;
using static System.Net.WebRequestMethods;

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

    [HttpGet("google-login")]
    public IActionResult GoogleLogin()
    {
        var props = new AuthenticationProperties
        {
            // simples: volta pro seu próprio endpoint de callback
            RedirectUri = Url.Action(nameof(GoogleResponse), "AuthGoogle")
        };
        return Challenge(props, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("/signin-google")]
    public async Task<IActionResult> GoogleResponse()
    {
        // Aqui você deve tentar autenticar no esquema do cookie, porque o middleware do Google no callback cria a identidade e a coloca no cookie

        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!result.Succeeded)
            return BadRequest("Autenticação com Google falhou");

        var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
        var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var nome = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(email))
            return BadRequest("Email não fornecido pelo Google.");

        // Resto do código de criar/consultar usuário e gerar token JWT

        var usuario = _context.AppUsers.FirstOrDefault(u => u.Email == email);
        if (usuario == null)
        {
            usuario = new AppUser
            {
                Name = nome,
                Email = email,
                Type = TipoUsuario.Client
            };
            _context.AppUsers.Add(usuario);
            await _context.SaveChangesAsync();
        }

        var token = _tokenService.GenerateToken(usuario.Email, usuario.Type);

        var frontendUrl = "https://barbergo-ui.onrender.com/login-success";
        return Redirect($"{frontendUrl}?token={token}");
    }
}
