using BarberGo.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace BarberGo.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailSender _emailService;

        public EmailController(IEmailSender emailService)
        {
            _emailService = emailService;
        }

        [HttpGet("teste")]
        public async Task<IActionResult> EnviarEmailTeste()
        {
            string destino = "themystocles@outlook.com"; // coloque seu e-mail aqui pra testar
            string assunto = "Não responda este email";
            string corpo = "<p>voce tem um agendamento marcado <strong>hoje</strong>as 9 horas.</p>";

            await _emailService.SendEmailAsync(destino, assunto, corpo);
            return Ok("E-mail enviado com sucesso!");
        }
    }
}
