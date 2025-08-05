using Domain.Entities;
using Application.DTOs;


namespace Application.Interfaces
{
    public interface IWeeklySchedule
    {
        Task<List<DateTime>> GetAvailableSlotsAsync(DateTime date, int? barberId = null);

        Task<WeeklySchedule> CreateNewSchedule(WeeklySchedule schedule);

        Task<WeeklySchedule> UpdateScheduleAsync(WeeklySchedule schedule);

        Task<List<BarberDto>> GetUserForType();

        Task <List <WeeklySchedule>> GetWeeklyScheduleByBarberId(int BarberId);
    }
}
