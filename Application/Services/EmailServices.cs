using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class EmailServices
    {
        private readonly IEmailSender _emailSender;
        private readonly IAppointmentQueryService _appointmentRepository;

        public EmailServices(IEmailSender emailSender, IAppointmentQueryService appointmentRepository)
        {
            _emailSender = emailSender;
            _appointmentRepository = appointmentRepository;
        }

        // ---------------------------
        // MÉTODOS EXISTENTES DE AGENDAMENTO
        // ---------------------------
        public async Task SendAppointmentNotificationToBarberAsync(Appointment appointment)
        {
            // Mantido como estava, buscando novamente no banco
            var appoint = await _appointmentRepository.GetAppointmentWithDetailsAsync(appointment.Id);
            if (appoint == null)
                throw new InvalidOperationException("Agendamento não encontrado.");

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

        // ---------------------------
        // NOVO MÉTODO DE CANCELAMENTO
        // ---------------------------
        // Não busca novamente no banco, usa o Appointment que já foi carregado antes do delete
        public async Task SendAppointmentCancellationNotificationToBarberAsync(Appointment appoint)
        {
            if (appoint == null || appoint.Barber == null || appoint.Client == null)
                throw new InvalidOperationException("Agendamento, barbeiro ou cliente não encontrado.");

            string destine = appoint.Barber.Email;
            string subject = "Agendamento Cancelado";
            string body = $@"
                <p>Olá <strong>{appoint.Barber.Name}</strong>,</p>
                <p>O cliente <strong>{appoint.Client.Name}</strong> cancelou o agendamento que estava marcado para <strong>{appoint.DateTime:dd/MM/yyyy HH:mm}</strong>.</p>
                <p>O horário agora está disponível para outros clientes.</p>";

            await _emailSender.SendEmailAsync(destine, subject, body);
        }

        // ---------------------------
        // MÉTODO FUTURO (RECUPERAÇÃO DE SENHA)
        // ---------------------------
        public async Task SendCodeRecoveryPassword()
        {
            // ainda vazio
        }
    }
}