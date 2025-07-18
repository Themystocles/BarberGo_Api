using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Api.Interfaces;

namespace Api.Entities
{
    public class AppUser : IEntity
    {
       
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; } 
        public string? PasswordHash { get; set; } 
        public string? GoogleId { get; set; } 
        public string? ProfilePictureUrl { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
        public TipoUsuario Type { get; set; }
        public string? RecoveryCode { get; set; }
        public DateTime? RecoveryCodeExpiration { get; set; }
        public bool IsMaster { get; set; } = false;
}

    public enum TipoUsuario
    {
        Client,
        Administrator
    }
}
