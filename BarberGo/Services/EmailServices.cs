using BarberGo.Data;
using BarberGo.Entities;
using BarberGo.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BarberGo.Services
{
    public class EmailServices
    {
        private readonly IEmailSender _emailSender;
        private readonly DataContext _dataContext;

        public EmailServices(IEmailSender emailSender, DataContext dataContext)
        {
            _emailSender = emailSender;
            _dataContext = dataContext;

        }

        public async Task SendAppointmentNotificationToBarberAsync(Appointment appointment)
        {
            var appoint = await _dataContext.Appointments
                .Include(c => c.Client)
                .Include(h => h.Haircut)
                .Include(a => a.Barber)
                .FirstOrDefaultAsync(a => a.Id == appointment.Id);

            string destine = appoint.Barber.Email;
            string subject = "Confirmação de agendamento";
            string body = $@"
                      <p>Olá <strong>{appoint.Barber.Name}</strong>,</p>

                       <p>Você tem um novo agendamento:</p>
                          <ul>
                            <li><strong>Cliente:</strong> {appoint.Client.Name}</li>
                            <li><strong>Corte:</strong> {appoint.Haircut.Name}</li>
                            <li><strong>Duração:</strong> {appoint.Haircut.Duracao} minutos</li>
                            <li><strong>Preço:</strong> R$ {appoint.Haircut.Preco:F2}</li>
                            <li><strong>Data e hora:</strong> {appoint.DateTime:dd/MM/yyyy HH:mm}</li>
                          </ul>



                       <p>Por favor, esteja preparado no horário agendado.</p>";

            await _emailSender.SendEmailAsync(destine, subject, body);




        }


    }
}
