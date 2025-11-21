using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Domain.Interfaces
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly string _apiKey;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public SendGridEmailSender(string apiKey, string fromEmail, string fromName = "Seu Sistema")
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _fromEmail = fromEmail ?? throw new ArgumentNullException(nameof(fromEmail));
            _fromName = fromName;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_fromEmail, _fromName);
            var to = new EmailAddress(toEmail);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: null, htmlContent: htmlBody);

            var response = await client.SendEmailAsync(msg);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Body.ReadAsStringAsync();
                throw new Exception($"Falha ao enviar e-mail: {response.StatusCode} - {body}");
            }
        }
    }
}
