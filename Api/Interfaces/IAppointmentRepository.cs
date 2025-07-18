using Api.Entities;
using Api.Entities.DTOs;

namespace Api.Interfaces
{
    public interface IAppointmentRepository
    {
        Task <List<MyAppointmentDto>> GetAppointmentsByUserId(int idUser);

        Task<List<MyAppointmentDto>> GetHistoryAppointment(int idUser, DateTime? date);

        Task<List<MyAppointmentDto>> GetMyAppointmentsHistorybyBarberId (int barberId, DateTime? date );
    }
}
