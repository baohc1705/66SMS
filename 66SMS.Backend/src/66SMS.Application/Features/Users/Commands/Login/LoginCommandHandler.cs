using _66SMS.Application.Abstractions.Security;
using _66SMS.Application.DTOs.Identity.Commands;
using _66SMS.Application.DTOs.Identity.Queries;
using _66SMS.Domain.Abstractions.Repositories.Sql;
using MediatR;

using _66SMS.Contracts.Shared;

namespace _66SMS.Application.Features.Users.Commands.Login
{
    public sealed record LoginCommand(
        string EmailOrUserName, 
        string Password, 
        string IpAddress, 
        string UserAgent) : IRequest<Result<LoginResponseDTO>>;

    public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponseDTO>>
    {
        private readonly IUserSqlRepository userSqlRepo;
        private readonly IUserAuthRepository userAuthRepo;
        private readonly IUserTokenRepository userTokenRepo;
        private readonly IJwtTokenService jwtService;

        public LoginCommandHandler(
            IUserSqlRepository userSqlRepo,
            IUserAuthRepository userAuthRepo,
            IUserTokenRepository userTokenRepo,
            IJwtTokenService jwtService)
        {
            this.userSqlRepo = userSqlRepo;
            this.userAuthRepo = userAuthRepo;
            this.userTokenRepo = userTokenRepo;
            this.jwtService = jwtService;
        }

        public async Task<Result<LoginResponseDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {

            // Tìm user theo email hoặc username
            var user = await userSqlRepo.FindByEmailAsync(request.EmailOrUserName, cancellationToken)
                    ?? await userSqlRepo.FindByUserNameAsync(request.EmailOrUserName, cancellationToken);

            if (user == null)
                return Result<LoginResponseDTO>.BadRequest("Invalid credentials.");

            // Kiểm tra lockout
            if (await userSqlRepo.IsLockedOutAsync(user, cancellationToken))
            {
                var lockoutEnd = await userSqlRepo.GetLockoutEndDateAsync(user, cancellationToken);
                return Result<LoginResponseDTO>.BadRequest($"Account is locked. Try again after {lockoutEnd:HH:mm dd/MM/yyyy} UTC.");
            }

            // Kiểm tra password
            var isValidPassword = await userAuthRepo.CheckPasswordAsync(user, request.Password, cancellationToken);
            if (!isValidPassword)
            {
                await userAuthRepo.IncrementAccessFailedCountAsync(user, cancellationToken);

                var failedCount = await userSqlRepo.GetAccessFailedCountAsync(user, cancellationToken);
                var maxAttempts = await userSqlRepo.GetMaxFailedAccessAttemptsAsync(cancellationToken);
                var remaining = maxAttempts - failedCount;

                return Result<LoginResponseDTO>.BadRequest("Invalid credentials."); // RemainingAttempts should ideally be returned in a ProblemDetails extension or data object, but we use standard BadRequest here.
            }

            // Kiểm tra 2FA
            if (await userSqlRepo.IsTwoFactorEnabledAsync(user, cancellationToken))
                return Result<LoginResponseDTO>.Success(new LoginResponseDTO { RequiresTwoFactor = true });

            // Login thành công
            await userAuthRepo.ResetAccessFailedCountAsync(user, cancellationToken);
            await userAuthRepo.UpdateLastLoginAsync(user, DateTime.UtcNow, cancellationToken);

            var roles = await userAuthRepo.GetUserRolesAsync(user, cancellationToken);
            var accessToken = jwtService.GenerateAccessToken(user, roles);
            var refreshToken = await userTokenRepo.GenerateAndStoreRefreshTokenAsync(
                user.Id, request.UserAgent, request.IpAddress, cancellationToken);

            return Result<LoginResponseDTO>.Success(new LoginResponseDTO
            {
                Succeeded = true,
                Token = accessToken,
                RefreshToken = refreshToken
            });
        }
    }
}
