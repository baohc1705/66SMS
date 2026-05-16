using _66SMS.Application.DTOs.Identity.Queries;
using _66SMS.Domain.Abstractions.Repositories.Sql;
using MediatR;

using _66SMS.Contracts.Shared;

namespace _66SMS.Application.Features.Users.Queries.GetProfile
{
    public sealed record GetProfileQuery(Guid UserId) : IRequest<Result<ProfileDTO>>;

    public sealed class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, Result<ProfileDTO>>
    {
        private readonly IUserSqlRepository userSqlRepo;

        public GetProfileQueryHandler(IUserSqlRepository userSqlRepo)
        {
            this.userSqlRepo = userSqlRepo;
        }

        public async Task<Result<ProfileDTO>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await userSqlRepo.FindByIdAsync(request.UserId);

            if (user == null)
                return Result<ProfileDTO>.NotFound("User profile not found.");

            return Result<ProfileDTO>.Success(new ProfileDTO
            {
                UserId = user.Id,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                UserName = user.UserName,
                LastLoginAt = user.LastLoginAt,
                ProfilePhotoUrl = user.ProfilePhotoUrl
            });
        }
    }
}
