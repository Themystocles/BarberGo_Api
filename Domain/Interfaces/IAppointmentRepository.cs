
using Domain.Entities;
using Domain.Entities.DTOs;

namespace Domain.Interfaces
{
    public interface IAppointmentRepository
    {
        Task <List<MyAppointmentDto>> GetAppointmentsByUserId(int idUser);

        Task<List<MyAppointmentDto>> GetHistoryAppointment(int idUser, DateTime? date);

        Task<List<MyAppointmentDto>> GetMyAppointmentsHistorybyBarberId (int barberId, DateTime? date );

        Task<Appointment?> GetAppointmentWithDetailsAsync(int id);
    }
}
