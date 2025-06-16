using System.ComponentModel.DataAnnotations;

namespace BarberGo.Entities.DTOs
{
    public class RegisterUserDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? Phone { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "As senhas não coincidem.")]
        public string ConfirmPassword { get; set; }

        public string? ProfilePictureUrl { get; set; }

        public TipoUsuario Type { get; set; } = TipoUsuario.Client;
    }
}
