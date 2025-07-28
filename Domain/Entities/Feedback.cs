using Domain.Entities.DTOs;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class Feedback : IEntity
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }  // Cliente
        public int BarberId { get; set; }   // Barbeiro

        public string Comment { get; set; }
        public int Rating { get; set; }

        public AppUser AppUser { get; set; }
        public AppUser Barber { get; set; }

    }
}
