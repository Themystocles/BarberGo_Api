using BarberGo.Entities;
using BarberGo.Entities.DTOs;

namespace BarberGo.Interfaces
{
    public interface IAppointmentRepository
    {
        Task <List<MyAppointmentDto>> GetAppointmentsByUserId(int idUser);
    }
}
