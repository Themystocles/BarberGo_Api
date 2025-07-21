using Domain.Entities;
using Domain.Entities.DTOs;

namespace Domain.Interfaces
{
    public interface ITodaysCustomers
    {
        Task<List<CustomerOfTheDayDto>> MyCustomersOfTheDay(DateTime date, int BarberId);
    }
}
