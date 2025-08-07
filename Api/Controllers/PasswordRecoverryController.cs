using Application.DTOs;
using Domain.Interfaces.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Api.Controllers
{
    public class PasswordRecoverryController : ControllerBase
    {
        private readonly IRecoveryPassword _recoveryPassword;
        public PasswordRecoverryController(IRecoveryPassword recoveryPassword)
        {
            _recoveryPassword = recoveryPassword;
            
        }
      
        [HttpGet("recoveryPassword/{email}")]
        public async Task SendCodeRecoveryPassword(string email)
        {
            await _recoveryPassword.SendRecoveryCodeAsync(email);
        }

        [HttpPost("redefinir-senha")]
        public async Task<IActionResult> RedefinirSenha([FromBody] ResetPasswordDto dto)
        {
            var sucesso = await _recoveryPassword.ResetPasswordWithCodeAsync(dto.Email, dto.Codigo, dto.NovaSenha);
            if (sucesso)
                return Ok(new { message = "Senha redefinida com sucesso." });
            else
                return BadRequest(new { message = "Código inválido, expirado ou dados incorretos." });
        }

    }
}
