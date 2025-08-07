using Persistence.Data;
using Domain.Entities;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Runtime.Serialization.DataContracts;

namespace Persistence.Repositories
{
    public class AppointmentRepository : IAppointmentQueryService
    {
        private readonly DataContext _dataContext;

        public AppointmentRepository(DataContext context)
        {
            _dataContext = context;
        }

        public async Task<List<MyAppointmentDto>> GetAppointmentsByUserId(int idUser)
        {
            return await _dataContext.Appointments
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

        public async Task<Appointment?> GetAppointmentWithDetailsAsync(int appointmentId)
        {
            return await _dataContext.Appointments
                .Include(c => c.Client)
                .Include(h => h.Haircut)
                .Include(b => b.Barber)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);
        }

        public async Task<List<MyAppointmentDto>> GetHistoryAppointment(int idUser, DateTime? date = null)
        {
            var query = _dataContext.Appointments
                .Where(a => a.ClientId == idUser && a.DateTime < DateTime.Now)
                .Include(a => a.Client)
                .Include(a => a.Barber)
                .Include(a => a.Haircut)
                .AsQueryable();

            if (date.HasValue)
            {

                query = query.Where(a => a.DateTime.Date == date.Value.Date);
            }

            return await query
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

        public async Task<List<MyAppointmentDto>> GetMyAppointmentsHistorybyBarberId(int barberId, DateTime? date)
        {
            var query = _dataContext.Appointments
               .Where(a => a.BarberId == barberId && a.DateTime < DateTime.Now)
               .Include(a => a.Client)
               .Include(a => a.Barber)
               .Include(a => a.Haircut)
               .AsQueryable();

            if (date.HasValue)
            {

                query = query.Where(a => a.DateTime.Date == date.Value.Date);
            }

            return await query
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
