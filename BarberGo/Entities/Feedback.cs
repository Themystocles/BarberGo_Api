namespace BarberGo.Entities
{
    public class Feedback
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; } 
        public int ClientId { get; set; }
        public int Rating { get; set; } 
        public string Comment { get; set; }
        public Appointment Appointment { get; set; }
        public AppUser Client { get; set; }
    }
}
