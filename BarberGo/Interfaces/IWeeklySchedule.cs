using BarberGo.Entities;

namespace BarberGo.Interfaces
{
    public interface IWeeklySchedule
    {
        Task<List<DateTime>> GetAvailableSlotsAsync(DateTime date, int? barberId = null);

        Task<WeeklySchedule> CreateNewSchedule(WeeklySchedule schedule);
    }
}
