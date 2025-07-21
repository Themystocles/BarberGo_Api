using Domain.Interfaces;

namespace Domain.Entities
{
    public class Haircut : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    
        public decimal Preco { get; set; }
        public TimeSpan Duracao { get; set; }

        public string? ImagePath { get; set; }



    }
}
