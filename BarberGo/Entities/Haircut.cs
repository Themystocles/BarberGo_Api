namespace BarberGo.Entities
{
    public class Haircut
    {
        public int Id { get; set; }
        public string Name { get; set; }  
        public decimal Preco { get; set; }
        public TimeSpan Duracao { get; set; }
    }
}
