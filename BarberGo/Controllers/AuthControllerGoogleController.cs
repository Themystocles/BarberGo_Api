﻿using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BarberGo.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
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

        if (string.IsNullOrEmpty(email))
            return BadRequest("Email não fornecido pelo Google.");

        // Verifica se o usuário já existe
        var usuario = _context.AppUsers.FirstOrDefault(u => u.Email == email);

        if (usuario == null)
        {
            // Cria novo usuário com perfil padrão
            usuario = new AppUser
            {
                Name = nome,
                Email = email,
                Type = TipoUsuario.Client // ou qualquer padrão
            };

            _context.AppUsers.Add(usuario);
            await _context.SaveChangesAsync();
        }

        var token = _tokenService.GenerateToken(usuario.Email, usuario.Type);

        var frontendUrl = "https://barbergo-ui.onrender.com/login-success";
        return Redirect($"{frontendUrl}?token={token}");
    }
}