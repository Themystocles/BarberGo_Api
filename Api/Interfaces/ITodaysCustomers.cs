using Api.Entities;
using Api.Entities.DTOs;

namespace Api.Interfaces
{
    public interface ITodaysCustomers
    {
        Task<List<CustomerOfTheDayDto>> MyCustomersOfTheDay(DateTime date, int BarberId);
    }
}
