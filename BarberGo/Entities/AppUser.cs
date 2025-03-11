namespace BarberGo.Entities
{
    public class AppUser
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
    }

    public enum TipoUsuario
    {
        Client,
        Administrator
    }
}
