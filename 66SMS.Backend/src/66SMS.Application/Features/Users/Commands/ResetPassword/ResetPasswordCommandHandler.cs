using _66SMS.Application.DTOs.Identity.Commands;
using _66SMS.Domain.Abstractions.Repositories.Sql;
using MediatR;

using _66SMS.Contracts.Shared;

namespace _66SMS.Application.Features.Users.Commands.ResetPassword
{
    public sealed record ResetPasswordCommand(
        Guid UserId, 
        string Token, 
        string NewPassword) : IRequest<Result<bool>>;

    public sealed class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<bool>>
    {
        private readonly IUserSqlRepository userSqlRepo;
        private readonly IUserAuthRepository userAuthRepo;

        public ResetPasswordCommandHandler(
            IUserSqlRepository userSqlRepo,
            IUserAuthRepository userAuthRepo)
        {
            this.userSqlRepo = userSqlRepo;
            this.userAuthRepo = userAuthRepo;
        }

        public async Task<Result<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userSqlRepo.FindByIdAsync(request.UserId);

            if (user == null)
                return Result<bool>.NotFound("User not found.");

            var success = await userAuthRepo.ResetPasswordAsync(
                user, request.Token, request.NewPassword, cancellationToken);
                
            if (!success)
                return Result<bool>.BadRequest("Password reset failed. Token may be invalid or expired.");
                
            return Result<bool>.Success(true, "Password reset successfully.");
        }
    }
}
