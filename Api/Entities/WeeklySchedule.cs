using Api.Interfaces;

namespace Api.Entities
{
    public class WeeklySchedule : IEntity
    {
        public int Id { get; set; }

        public DayOfWeek DayOfWeek { get; set; } 
        public TimeSpan StartTime { get; set; }  
        public TimeSpan EndTime { get; set; }    
        public int IntervalMinutes { get; set; } 
        public int? BarberId { get; set; }      
        public AppUser? Barber { get; set; }
    }
}
