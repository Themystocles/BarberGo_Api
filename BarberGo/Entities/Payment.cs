namespace BarberGo.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; } 
        public int ClientId { get; set; } 
        public decimal Amount { get; set; } 
        public DateTime PaymentDate { get; set; } 
        public string PaymentMethod { get; set; } 
        public bool IsPaid { get; set; }
        public Appointment Appointment { get; set; } 
        public AppUser Client { get; set; } 
    }
}
