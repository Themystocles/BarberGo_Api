using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using Persistence.Data;

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
        var redirectUrl = Url.Action("GoogleResponse");
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google-response")]
    public async Task<IActionResult> GoogleResponse()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (!result.Succeeded)
            return BadRequest("Autenticação com Google falhou");

        var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
        var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var nome = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var fotoPerfil = claims?.FirstOrDefault(c => c.Type == "picture")?.Value;

        Console.WriteLine("Foto de perfil do Google: " + fotoPerfil);

        if (string.IsNullOrEmpty(email))
            return BadRequest("Email não fornecido pelo Google.");

        // Busca usuário existente
        var usuario = _context.AppUsers.FirstOrDefault(u => u.Email == email);

        if (usuario == null)
        {
            // Cria novo usuário
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
        else
        {
            

            if (string.IsNullOrEmpty(usuario.ProfilePictureUrl))
            {
                usuario.ProfilePictureUrl = fotoPerfil;
                _context.AppUsers.Update(usuario);
                await _context.SaveChangesAsync();
            }
            
        }

        var token = _tokenService.GenerateToken(usuario.Email, usuario.Type);

        var frontendUrl = "https://barbergo-ui.onrender.com/login-success";
        return Redirect($"{frontendUrl}?token={token}");

        //teste
    }
}