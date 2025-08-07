
using Domain.Entities;
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IAppointmentQueryService
    {
        Task <List<MyAppointmentDto>> GetAppointmentsByUserId(int idUser);

        Task<List<MyAppointmentDto>> GetHistoryAppointment(int idUser, DateTime? date);

        Task<List<MyAppointmentDto>> GetMyAppointmentsHistorybyBarberId (int barberId, DateTime? date );

        Task<Appointment?> GetAppointmentWithDetailsAsync(int id);
    }
}
