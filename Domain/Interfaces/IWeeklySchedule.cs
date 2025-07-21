using Domain.Entities;
using Domain.Entities.DTOs;

namespace Domain.Interfaces
{
    public interface IWeeklySchedule
    {
        Task<List<DateTime>> GetAvailableSlotsAsync(DateTime date, int? barberId = null);

        Task<WeeklySchedule> CreateNewSchedule(WeeklySchedule schedule);

        Task<WeeklySchedule> UpdateScheduleAsync(WeeklySchedule schedule);

        Task <List<BarberDto>> GetUserForType();

        Task <List <WeeklySchedule>> GetWeeklyScheduleByBarberId(int BarberId);
    }
}
