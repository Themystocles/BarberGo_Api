using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.DTOs
{
    public class RegisterUserDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? Phone { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "A confirmação de senha é obrigatória.")]
        [Compare("Password", ErrorMessage = "As senhas não coincidem.")]
        public string ConfirmPassword { get; set; }

        public string? ProfilePictureUrl { get; set; }

        public TipoUsuario Type { get; set; } = TipoUsuario.Client;

        public string? RecoveryCode { get; set; }
    }
}
