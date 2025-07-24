using Domain.Interfaces;
using Domain.Entities;

public class EmailConfirmationServices
{
    private readonly IEmailVerificationRepository _emailVerificationRepository;
    private readonly IEmailSender _emailSender;

    public EmailConfirmationServices(IEmailVerificationRepository emailVerificationRepository, IEmailSender emailSender)
    {
        _emailVerificationRepository = emailVerificationRepository;
        _emailSender = emailSender;
    }

    public async Task<string> SendCodeverifyEmail(string email)
    {
        if (email == null) throw new ArgumentNullException(nameof(email));

        var codigo = new Random().Next(100000, 999999).ToString();
        var validade = DateTime.UtcNow.AddMinutes(15);

        var emailExist = await _emailVerificationRepository.GetByEmailAsync(email);

        if (emailExist == null)
        {
            var emailVerification = new EmailVerification
            {
                Email = email,
                Code = codigo,
                Expiration = validade,
                Verified = false
            };

            await _emailVerificationRepository.AddAsync(emailVerification);
        }
        else if (!emailExist.Verified)
        {
            emailExist.Code = codigo;
            emailExist.Expiration = validade;
            await _emailVerificationRepository.UpdateAsync(emailExist);
        }
        else
        {
            throw new InvalidOperationException("E-mail já existe, favor fazer login.");
        }

        await _emailSender.SendEmailAsync(email, "Código de recuperação", $"Seu código é: {codigo}");
        await _emailVerificationRepository.SaveChangesAsync();

        return codigo;
    }

    public async Task<bool> VerificationEmail(string email, string code)
    {
        var verification = await _emailVerificationRepository.GetByEmailAsync(email);

        if (verification == null || verification.Expiration < DateTime.UtcNow || verification.Code != code)
            return false;

        verification.Verified = true;
        await _emailVerificationRepository.SaveChangesAsync();

        return true;
    }
}
