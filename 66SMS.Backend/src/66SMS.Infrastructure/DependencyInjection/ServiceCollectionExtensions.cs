using _66SMS.Application.Abstractions.Security;
using _66SMS.Application.Abstractions.Services;
using _66SMS.Infrastructure.Mails;
using _66SMS.Infrastructure.Security.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace _66SMS.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Đăng ký các services thuộc Infrastructure layer:
        /// - JWT token service (signing + generation)
        /// - Email service (SMTP)
        /// </summary>
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Bind JwtSettings từ appsettings.json
            services.Configure<JwtSettings>(
                configuration.GetSection(JwtSettings.SectionName));

            // Bind MailSettings từ appsettings.json
            services.Configure<MailSetting>(
                configuration.GetSection(MailSetting.SectionName));

            // JWT token generation service
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            // Email service (SMTP – infrastructure concern)
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
