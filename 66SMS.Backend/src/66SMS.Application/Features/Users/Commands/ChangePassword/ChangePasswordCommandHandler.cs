using _66SMS.Application.DTOs.Identity.Commands;
using _66SMS.Domain.Abstractions.Repositories.Sql;
using MediatR;

using _66SMS.Contracts.Shared;

namespace _66SMS.Application.Features.Users.Commands.ChangePassword
{
    public sealed record ChangePasswordCommand(
        Guid UserId, 
        string CurrentPassword, 
        string NewPassword) : IRequest<Result<bool>>;

    public sealed class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<bool>>
    {
        private readonly IUserSqlRepository userSqlRepo;
        private readonly IUserAuthRepository userAuthRepo;

        public ChangePasswordCommandHandler(
            IUserSqlRepository userSqlRepo,
            IUserAuthRepository userAuthRepo)
        {
            this.userSqlRepo = userSqlRepo;
            this.userAuthRepo = userAuthRepo;
        }

        public async Task<Result<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userSqlRepo.FindByIdAsync(request.UserId);
            if (user == null)
                return Result<bool>.NotFound("User not found.");
            
            var success = await userAuthRepo.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword, cancellationToken);
            if (!success)
                return Result<bool>.BadRequest("Failed to change password. Check your current password.");
                
            return Result<bool>.Success(true, "Password changed successfully.");
        }
    }
}
