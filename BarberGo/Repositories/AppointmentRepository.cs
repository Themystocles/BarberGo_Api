using BarberGo.Data;
using BarberGo.Entities;
using BarberGo.Entities.DTOs;
using BarberGo.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Runtime.Serialization.DataContracts;

namespace BarberGo.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly DataContext _dataContext;

        public AppointmentRepository(DataContext context)
        {
            _dataContext = context;
        }

        public async Task<List<MyAppointmentDto>> GetAppointmentsByUserId(int idUser)
        {
           return  await _dataContext.Appointments
                .Where(a => a.ClientId == idUser && a.DateTime >= DateTime.Today)
                .Include(a => a.Client)
                .Include(a => a.Barber)
                .Include(a => a.Haircut)
                 .Select(a => new MyAppointmentDto
                 {
                     id = a.Id,
                     ClientName = a.Client.Name,
                     ClientPhone = a.Client.Phone,
                     HaircutName = a.Haircut.Name,
                     HaircutPreco = a.Haircut.Preco,
                     BarberName = a.Barber.Name,
                     DateTime = a.DateTime,
                     Status = a.Status
                 })
                .ToListAsync();

                


        }
    }
}
