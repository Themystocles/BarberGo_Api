namespace Domain.Interfaces
{
    namespace Domain.Interfaces
    {
        public interface IRecoveryPassword
        {
            Task<bool> SendRecoveryCodeAsync(string email);
            Task<bool> ResetPasswordWithCodeAsync(string email, string code, string newPassword);
        }
    }
}
