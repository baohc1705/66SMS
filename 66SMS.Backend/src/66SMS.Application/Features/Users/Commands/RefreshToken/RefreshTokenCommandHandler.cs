using _66SMS.Application.Abstractions.Security;
using _66SMS.Application.DTOs.Identity.Commands;
using _66SMS.Application.DTOs.Identity.Queries;
using _66SMS.Domain.Abstractions.Repositories.Sql;
using MediatR;

using _66SMS.Contracts.Shared;

namespace _66SMS.Application.Features.Users.Commands.RefreshToken
{
    public sealed record RefreshTokenCommand(
        string RefreshToken, 
        string IpAddress, 
        string UserAgent) : IRequest<Result<RefreshTokenResponseDTO>>;

    public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponseDTO>>
    {
        private readonly IUserTokenRepository userTokenRepo;
        private readonly IUserSqlRepository userSqlRepo;
        private readonly IUserAuthRepository userAuthRepo;
        private readonly IJwtTokenService jwtService;

        public RefreshTokenCommandHandler(
            IUserTokenRepository userTokenRepo,
            IUserSqlRepository userSqlRepo,
            IUserAuthRepository userAuthRepo,
            IJwtTokenService jwtService)
        {
            this.userTokenRepo = userTokenRepo;
            this.userSqlRepo = userSqlRepo;
            this.userAuthRepo = userAuthRepo;
            this.jwtService = jwtService;
        }

        public async Task<Result<RefreshTokenResponseDTO>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var existingToken = await userTokenRepo.GetRefreshTokenAsync(request.RefreshToken, cancellationToken);

            if (existingToken == null || !existingToken.IsActive)
                return Result<RefreshTokenResponseDTO>.Unauthorized("Invalid or expired refresh token.");

            var user = await userSqlRepo.FindByIdAsync(existingToken.UserId);
            if (user == null)
                return Result<RefreshTokenResponseDTO>.NotFound("User not found.");

            // Revoke token cũ
            await userTokenRepo.RevokeRefreshTokenAsync(existingToken, request.IpAddress, cancellationToken);

            // Tạo token mới
            var roles = await userAuthRepo.GetUserRolesAsync(user, cancellationToken);
            var newAccessToken = jwtService.GenerateAccessToken(user, roles);
            var newRefreshToken = await userTokenRepo.GenerateAndStoreRefreshTokenAsync(
                user.Id, request.UserAgent, request.IpAddress, cancellationToken);

            return Result<RefreshTokenResponseDTO>.Success(new RefreshTokenResponseDTO
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
