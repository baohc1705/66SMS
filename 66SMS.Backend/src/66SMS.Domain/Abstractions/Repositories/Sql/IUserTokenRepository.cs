using _66SMS.Domain.Entities;

namespace _66SMS.Domain.Abstractions.Repositories.Sql
{
    public interface IUserTokenRepository
    {
        Task<string> GenerateAndStoreRefreshTokenAsync(Guid userId,string userAgent,string ipAddress,CancellationToken ct = default);
        Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken ct = default);
        Task RevokeRefreshTokenAsync(RefreshToken refreshToken,string ipAddress,CancellationToken ct = default);
        Task RevokeAllUserTokensAsync(Guid userId, CancellationToken ct = default);
    }
}
