using BarberGo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BarberGo.Entities
{
    public class Haircut : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Precision(18, 2)]
        public decimal Preco { get; set; }
        public TimeSpan Duracao { get; set; }

        public string? ImagePath { get; set; }



    }
}
