using BarberGo.Entities;
using BarberGo.Entities.DTOs;
using BarberGo.Interfaces;

namespace BarberGo.Services
{
    public class AppointmentServices
    {
        private readonly IAppointmentRepository _repository;

        public AppointmentServices(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        public async Task <List<MyAppointmentDto>> GetAppointmentByIdUser(int idUser)
        {
            if (idUser == 0)
            {
                throw new ArgumentNullException(nameof(idUser));
            }
            if(idUser == null) 
            {
                throw new ArgumentNullException(nameof(idUser));
            }
            var Appointment = await _repository.GetAppointmentsByUserId(idUser);

            return Appointment;

        }


    }
}
