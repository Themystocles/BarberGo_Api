using Domain.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class ResendEmailSender : IEmailSender
{
    private readonly EmailSettings _settings;
    private readonly HttpClient _httpClient;

    public ResendEmailSender(
        IOptions<EmailSettings> settings,
        HttpClient httpClient)
    {
        _settings = settings.Value;
        _httpClient = httpClient;

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _settings.ApiKey);
    }

    public async Task SendEmailAsync(
        string toEmail,
        string subject,
        string htmlBody)
    {
        var body = new
        {
            from = $"{_settings.FromName} <{_settings.FromEmail}>",
            to = toEmail,
            subject = subject,
            html = htmlBody
        };

        var json = JsonSerializer.Serialize(body);

        var content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync(
            "https://api.resend.com/emails",
            content);

        if (!response.IsSuccessStatusCode)
        {
            var erro = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao enviar email: {erro}");
        }
    }
}
