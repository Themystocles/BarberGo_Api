using Api.Data;
using Api.Entities;
using Api.Entities.DTOs;
using Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class TodaysCustomers : ITodaysCustomers
    {
        private readonly DataContext _context;

        public TodaysCustomers(DataContext context)
        {
            _context = context;
        }
        public async Task<List<CustomerOfTheDayDto>> MyCustomersOfTheDay(DateTime date, int barberid)
        {
            var startDate = date.Date.ToUniversalTime();
            var endDate = startDate.AddDays(1);

            return await _context.Appointments
                .Include(a => a.Client)
                .Include(a => a.Haircut)
                .Where(a => a.BarberId == barberid
                         && a.DateTime >= startDate
                         && a.DateTime < endDate)
                .Select(a => new CustomerOfTheDayDto
                {
                    id = a.Id,
                    ClientName = a.Client.Name,
                    ClientPhone = a.Client.Phone,
                    HaircutName = a.Haircut.Name,
                    HaircutPreco = a.Haircut.Preco,
                    DateTime = a.DateTime,
                    Status = a.Status
                })
                .ToListAsync();
        }






    }
}
