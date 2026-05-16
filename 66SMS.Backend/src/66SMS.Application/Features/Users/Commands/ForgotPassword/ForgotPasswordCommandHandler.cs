using _66SMS.Application.Abstractions.Services;
using _66SMS.Application.DTOs.Identity.Commands;
using _66SMS.Application.DTOs.Identity.Queries;
using _66SMS.Domain.Abstractions.Repositories.Sql;
using MediatR;

using _66SMS.Contracts.Shared;

namespace _66SMS.Application.Features.Users.Commands.ForgotPassword
{
    public sealed record ForgotPasswordCommand(string Email) : IRequest<Result<ForgotPasswordResponseDTO>>;

    public sealed class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result<ForgotPasswordResponseDTO>>
    {
        private readonly IUserSqlRepository userSqlRepo;
        private readonly IUserAuthRepository userAuthRepo;
        private readonly IEmailService emailService;

        public ForgotPasswordCommandHandler(
            IUserSqlRepository userSqlRepo,
            IUserAuthRepository userAuthRepo,
            IEmailService emailService)
        {
            this.userSqlRepo = userSqlRepo;
            this.userAuthRepo = userAuthRepo;
            this.emailService = emailService;
        }

        public async Task<Result<ForgotPasswordResponseDTO>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userSqlRepo.FindByEmailAsync(request.Email, cancellationToken);

            // Trả về null nhưng không tiết lộ email có tồn tại không (security)
            if (user == null)
                return Result<ForgotPasswordResponseDTO>.Success(null!, "If the email exists, a password reset link has been sent.");

            var token = await userAuthRepo.GeneratePasswordResetTokenAsync(user, cancellationToken);
            if (string.IsNullOrEmpty(token))
                return Result<ForgotPasswordResponseDTO>.BadRequest("Failed to generate token.");

            // Gửi email reset password
            var resetLink = $"https://localhost:7777/reset-password?userId={user.Id}&token={Uri.EscapeDataString(token)}";
            await emailService.SendPasswordResetAsync(
                user.Email!,
                user.UserName ?? user.Email!,
                resetLink,
                cancellationToken);

            return Result<ForgotPasswordResponseDTO>.Success(new ForgotPasswordResponseDTO
            {
                UserId = user.Id,
                Token = token
            }, "If the email exists, a password reset link has been sent.");
        }
    }
}
