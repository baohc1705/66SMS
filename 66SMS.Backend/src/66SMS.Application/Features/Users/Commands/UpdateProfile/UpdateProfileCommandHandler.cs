using _66SMS.Application.DTOs.Identity.Commands;
using _66SMS.Domain.Abstractions.Repositories.Sql;
using MediatR;

using _66SMS.Contracts.Shared;

namespace _66SMS.Application.Features.Users.Commands.UpdateProfile
{
    public sealed record UpdateProfileCommand(
        Guid UserId, 
        string FullName, 
        string PhoneNumber, 
        string? ProfilePhotoUrl) : IRequest<Result<bool>>;

    public sealed class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result<bool>>
    {
        private readonly IUserSqlRepository userSqlRepo;
        private readonly IUserAuthRepository userAuthRepo;

        public UpdateProfileCommandHandler(
            IUserSqlRepository userSqlRepo,
            IUserAuthRepository userAuthRepo)
        {
            this.userSqlRepo = userSqlRepo;
            this.userAuthRepo = userAuthRepo;
        }

        public async Task<Result<bool>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await userSqlRepo.FindByIdAsync(request.UserId);

            if (user == null)
                return Result<bool>.NotFound("User not found.");

            user.FullName = request.FullName;
            user.PhoneNumber = request.PhoneNumber;
            user.ProfilePhotoUrl = request.ProfilePhotoUrl;

            var updated = await userAuthRepo.UpdateUserAsync(user, cancellationToken);
            if (!updated)
                return Result<bool>.BadRequest("Failed to update profile.");
                
            return Result<bool>.Success(true, "Profile updated successfully.");
        }
    }
}
