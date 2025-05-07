namespace BarberGo.Entities.DTOs
{
    
   public class CustomerOfTheDayDto
    {
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public string HaircutName { get; set; }
        public decimal HaircutPreco { get; set; }
        public DateTime DateTime { get; set; }
        public string Status { get; set; }
    }
}
