using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Application.Interfaces;

[ApiController]
[Route("auth")]
public class AuthGoogleController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthGoogleController(IAuthService authService)
    {
        _authService = authService;
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

        if (string.IsNullOrEmpty(email))
            return BadRequest("Email não fornecido pelo Google.");

        var token = await _authService.AutenticarOuRegistrarUsuarioGoogleAsync(email, nome, fotoPerfil);

        var frontendUrl = "https://barbergo-ui.onrender.com/login-success";
        return Redirect($"{frontendUrl}?token={token}");
    }
}
