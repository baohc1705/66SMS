using _66SMS.Application.Abstractions.Services;
using _66SMS.Application.DTOs.Identity.Commands;
using _66SMS.Domain.Abstractions.Repositories.Sql;
using MediatR;

using _66SMS.Contracts.Shared;

namespace _66SMS.Application.Features.Users.Commands.ConfirmEmail
{
    public sealed record ConfirmEmailCommand(Guid UserId, string Token) : IRequest<Result<bool>>;

    public sealed class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Result<bool>>
    {
        private readonly IUserSqlRepository userSqlRepo;
        private readonly IUserAuthRepository userAuthRepo;
        private readonly IEmailService emailService;

        public ConfirmEmailCommandHandler(
            IUserSqlRepository userSqlRepo,
            IUserAuthRepository userAuthRepo,
            IEmailService emailService)
        {
            this.userSqlRepo = userSqlRepo;
            this.userAuthRepo = userAuthRepo;
            this.emailService = emailService;
        }

        public async Task<Result<bool>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await userSqlRepo.FindByIdAsync(request.UserId);

            if (user == null)
                return Result<bool>.NotFound("User not found.");

            var confirmed = await userAuthRepo.VerifyConfirmationEmailAsync(
                user, request.Token, cancellationToken);

            if (confirmed)
            {
                // Gửi welcome email sau khi xác nhận thành công
                await emailService.SendWelcomeEmailAsync(
                    user.Email!,
                    user.UserName ?? user.Email!,
                    cancellationToken);
                    
                return Result<bool>.Success(true, "Email confirmed successfully. Welcome!");
            }

            return Result<bool>.BadRequest("Email confirmation failed. Token may be invalid or expired.");
        }
    }
}
