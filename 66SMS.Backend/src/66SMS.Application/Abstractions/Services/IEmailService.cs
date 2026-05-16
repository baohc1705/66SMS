namespace _66SMS.Application.Abstractions.Services
{
    public interface IEmailService
    {
        Task SendEmailConfirmationAsync(string toEmail, string userName, string confirmationLink, CancellationToken ct = default);
        Task SendPasswordResetAsync(string toEmail, string userName, string resetLink, CancellationToken ct = default);
        Task SendWelcomeEmailAsync(string toEmail, string userName, CancellationToken ct = default);
    }
}
