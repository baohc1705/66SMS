using _66SMS.Domain.Abstractions.Repositories.Sql;
using MediatR;

using _66SMS.Contracts.Shared;

namespace _66SMS.Application.Features.Users.Commands.RevokeToken
{
    public sealed record RevokeTokenCommand(string RefreshToken, string IpAddress) : IRequest<Result<bool>>;

    public sealed class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, Result<bool>>
    {
        private readonly IUserTokenRepository userTokenRepo;

        public RevokeTokenCommandHandler(IUserTokenRepository userTokenRepo)
        {
            this.userTokenRepo = userTokenRepo;
        }

        public async Task<Result<bool>> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            var token = await userTokenRepo.GetRefreshTokenAsync(request.RefreshToken, cancellationToken);

            if (token == null || !token.IsActive)
                return Result<bool>.NotFound("Token not found or already revoked.");

            await userTokenRepo.RevokeRefreshTokenAsync(token, request.IpAddress, cancellationToken);
            return Result<bool>.Success(true, "Token revoked successfully.");
        }
    }
}
