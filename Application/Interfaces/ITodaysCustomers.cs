using Domain.Entities;
using Application.DTOs;

namespace Domain.Interfaces
{
    public interface ITodaysCustomers
    {
        Task<List<CustomerOfTheDayDto>> MyCustomersOfTheDay(DateTime date, int BarberId);
    }
}
