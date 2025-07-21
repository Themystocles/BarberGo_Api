namespace Api.DTOs
{
    public class AppointmentDto
    {
        public int ClientId { get; set; }
        public int HaircutId { get; set; }
        public int BarberId { get; set; }
        public DateTime DateTime { get; set; }
        public string Status { get; set; }
    }
}
