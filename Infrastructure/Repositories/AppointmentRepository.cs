using Persistence.Data;
using Domain.Entities;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class AppointmentRepository : IAppointmentQueryService
    {
        private readonly DataContext _dataContext;

        public AppointmentRepository(DataContext context)
        {
            _dataContext = context;
        }

        // 🔥 FUTUROS (inclui hoje inteiro)
        public async Task<List<MyAppointmentDto>> GetAppointmentsByUserId(int idUser)
        {
            // UTC do servidor
            var utcNow = DateTime.UtcNow;

            // Converte para horário local (Brasil)
            var localNow = utcNow.AddHours(-3);

            // Pega apenas a DATA (ignora hora)
            var today = localNow.Date;

            return await _dataContext.Appointments
                .Where(a => a.ClientId == idUser && a.DateTime.Date >= today)
                .Select(a => new MyAppointmentDto
                {
                    id = a.Id,
                    ClientName = a.Client.Name,
                    ClientPhone = a.Client.Phone,
                    HaircutName = a.Haircut.Name,
                    HaircutPreco = a.Haircut.Preco,
                    BarberName = a.Barber.Name,
                    BarberPhone = a.Barber.Phone,
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

        // 🔥 HISTÓRICO (antes de hoje)
        public async Task<List<MyAppointmentDto>> GetHistoryAppointment(int idUser, DateTime? date = null)
        {
            var utcNow = DateTime.UtcNow;
            var localNow = utcNow.AddHours(-3);
            var today = localNow.Date;

            var query = _dataContext.Appointments
                .Where(a => a.ClientId == idUser && a.DateTime.Date < today)
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

        // 🔥 HISTÓRICO DO BARBEIRO
        public async Task<List<MyAppointmentDto>> GetMyAppointmentsHistorybyBarberId(int barberId, DateTime? date)
        {
            var utcNow = DateTime.UtcNow;
            var localNow = utcNow.AddHours(-3);
            var today = localNow.Date;

            var query = _dataContext.Appointments
                .Where(a => a.BarberId == barberId && a.DateTime.Date < today)
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