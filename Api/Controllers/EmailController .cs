using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailSender _emailService;

        public EmailController(IEmailSender emailService)
        {
            _emailService = emailService;
        }

        [HttpGet("teste")]
        public async Task<IActionResult> EnviarEmailTeste()
        {
            string destino = "themystocles21@gmail.com";
            string assunto = "Não responda este email";
            string corpo = "<p>Você tem um agendamento marcado <strong>hoje</strong> às 9 horas.</p>";

            await _emailService.SendEmailAsync(destino, assunto, corpo);

            return Ok("E-mail enviado com sucesso!");
        }
    }
}
