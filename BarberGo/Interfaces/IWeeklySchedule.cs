namespace BarberGo.Interfaces
{
    public interface IWeeklySchedule
    {
        Task<List<DateTime>> GetAvailableSlotsAsync(DateTime date, int? barberId = null);
    }
}
