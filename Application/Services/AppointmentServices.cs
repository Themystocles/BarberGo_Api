using Application.DTOs;
using Application.Interfaces;

namespace Application.Services
{
    public class AppointmentServices
    {
        private readonly IAppointmentQueryService _repository;

        public AppointmentServices(IAppointmentQueryService repository)
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
