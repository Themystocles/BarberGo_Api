using BarberGo.Entities;
using BarberGo.Entities.DTOs;

namespace BarberGo.Interfaces
{
    public interface ITodaysCustomers
    {
        Task<List<CustomerOfTheDayDto>> MyCustomersOfTheDay(DateTime date, int BarberId);
    }
}
