using BarberGo.Entities;
using BarberGo.Entities.DTOs;

namespace BarberGo.Interfaces
{
    public interface IAppointmentRepository
    {
        Task <List<MyAppointmentDto>> GetAppointmentsByUserId(int idUser);

        Task<List<MyAppointmentDto>> GetHistoryAppointment(int idUser, DateTime? date);

        Task<List<MyAppointmentDto>> GetMyAppointmentsHistorybyBarberId (int barberId, DateTime? date );
    }
}
