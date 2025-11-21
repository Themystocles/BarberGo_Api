using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

public class SendGridEmailSender : IEmailSender
{
    private readonly EmailSettings _settings;
    private readonly SendGridClient _client;

    public SendGridEmailSender(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
        _client = new SendGridClient(_settings.ApiKey);
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        var from = new EmailAddress(_settings.FromEmail, _settings.FromName);
        var to = new EmailAddress(toEmail);

        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: null, htmlContent: htmlBody);

        var response = await _client.SendEmailAsync(msg);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Erro ao enviar email. Status: {response.StatusCode}");
    }}
