using BarberGo.Interfaces;
using Microsoft.AspNetCore.Routing;

namespace BarberGo.Entities
{
    public class Appointment : IEntity
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int HaircutId { get; set; }
        public int BarberId { get; set; }
        public DateTime DateTime { get; set; }
        public string Status { get; set; }

      
        public AppUser? Client { get; set; }
        public Haircut? Haircut { get; set; }
        public AppUser? Barber { get; set; }
    }

    }

