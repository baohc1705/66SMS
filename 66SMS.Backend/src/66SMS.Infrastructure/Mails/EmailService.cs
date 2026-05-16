using _66SMS.Application.Abstractions.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;

namespace _66SMS.Infrastructure.Mails
{
    /// <summary>
    /// MailKit-based email service implementation.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly MailSetting _settings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<MailSetting> options, ILogger<EmailService> logger)
        {
            _settings = options.Value;
            _logger = logger;
        }

        public async Task SendEmailConfirmationAsync(string toEmail, string userName, string confirmationLink, CancellationToken ct = default)
        {
            var subject = "Xác nhận địa chỉ email của bạn";
            var body = $"""
                <h2>Xin chào {userName},</h2>
                <p>Cảm ơn bạn đã đăng ký tài khoản tại <strong>66SMS</strong>.</p>
                <p>Vui lòng nhấn vào nút bên dưới để xác nhận địa chỉ email của bạn:</p>
                <p><a href="{confirmationLink}" style="background:#6366f1;color:white;padding:12px 24px;border-radius:6px;text-decoration:none;">Xác nhận Email</a></p>
                <p>Liên kết này sẽ hết hạn sau 24 giờ.</p>
                <hr/>
                <p style="color:#888;font-size:12px;">Nếu bạn không đăng ký tài khoản, hãy bỏ qua email này.</p>
                """;

            await SendAsync(toEmail, subject, body, ct);
        }

        public async Task SendPasswordResetAsync(string toEmail, string userName, string resetLink, CancellationToken ct = default)
        {
            var subject = "Đặt lại mật khẩu";
            var body = $"""
                <h2>Xin chào {userName},</h2>
                <p>Chúng tôi nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn.</p>
                <p>Nhấn vào nút bên dưới để đặt lại mật khẩu:</p>
                <p><a href="{resetLink}" style="background:#ef4444;color:white;padding:12px 24px;border-radius:6px;text-decoration:none;">Đặt lại mật khẩu</a></p>
                <p>Liên kết này sẽ hết hạn sau 1 giờ.</p>
                <hr/>
                <p style="color:#888;font-size:12px;">Nếu bạn không yêu cầu đặt lại mật khẩu, hãy bỏ qua email này.</p>
                """;

            await SendAsync(toEmail, subject, body, ct);
        }

        public async Task SendWelcomeEmailAsync(string toEmail, string userName, CancellationToken ct = default)
        {
            var subject = "Chào mừng bạn đến với 66SMS!";
            var body = $"""
                <h2>Chào mừng {userName}!</h2>
                <p>Tài khoản của bạn đã được xác nhận thành công.</p>
                <p>Bây giờ bạn có thể đăng nhập và sử dụng đầy đủ các tính năng của <strong>66SMS</strong>.</p>
                <hr/>
                <p style="color:#888;font-size:12px;">© 2024 66SMS. All rights reserved.</p>
                """;

            await SendAsync(toEmail, subject, body, ct);
        }

        private async Task SendAsync(string toEmail, string subject, string htmlBody, CancellationToken ct)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
            message.To.Add(new MailboxAddress(toEmail, toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, ct);
                await client.AuthenticateAsync(_settings.User, _settings.Password, ct);
                await client.SendAsync(message, ct);
                await client.DisconnectAsync(true, ct);

                _logger.LogInformation("Email sent to {Email} with subject '{Subject}' using MailKit", toEmail, subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email} using MailKit", toEmail);
            }
        }
    }
}
