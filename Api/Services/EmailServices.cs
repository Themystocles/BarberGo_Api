using Persistence.Data;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Api.Services
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

            // E-mail para o BARBEIRO

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

            // E-mail para o CLIENTE
            string clientEmail = appoint.Client.Email;
            string subjectToClient = "Seu agendamento foi confirmado!";
            string bodyToClient = $@"
        <p>Olá <strong>{appoint.Client.Name}</strong>,</p>
        <p>Seu agendamento foi confirmado com sucesso! Veja os detalhes abaixo:</p>
        <ul>
            <li><strong>Barbeiro:</strong> {appoint.Barber.Name}</li>
            <li><strong>Corte:</strong> {appoint.Haircut.Name}</li>
            <li><strong>Duração:</strong> {appoint.Haircut.Duracao} minutos</li>
            <li><strong>Preço:</strong> R$ {appoint.Haircut.Preco:F2}</li>
            <li><strong>Data e hora:</strong> {appoint.DateTime:dd/MM/yyyy HH:mm}</li>
        </ul>
        <p>Nos vemos em breve!</p>";

            await _emailSender.SendEmailAsync(clientEmail, subjectToClient, bodyToClient);




        }

        public async Task SendCodeRecoveryPassword()
        {

        }


    }
}
