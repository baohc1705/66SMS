using _66SMS.Domain.Abstractions.Repositories.Sql;
using _66SMS.Domain.Entities;
using _66SMS.Persistence.Configurations.Identity;
using _66SMS.Persistence.Repositories.Sql.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace _66SMS.Persistence.Repositories.Sql
{
    public class UserTokenRepository : IUserTokenRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext context;

        public UserTokenRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        public async Task<string> GenerateAndStoreRefreshTokenAsync(Guid userId, string userAgent, string ipAddress, CancellationToken ct = default)
        {
            await RevokeAllRefreshTokensAsync(userId, userAgent, ipAddress);
            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                UserAgent = userAgent,
                Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedByIp = ipAddress
            };

            context.RefreshTokens.Add(refreshToken);
            await context.SaveChangesAsync();
            return refreshToken.Token;
        }


        public async Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken ct = default)
        {
            return await context.Set<RefreshToken>().AsNoTracking().FirstOrDefaultAsync(t => t.Token == token, ct);
        }
        public async Task RevokeRefreshTokenAsync(RefreshToken refreshToken, string ipAddress, CancellationToken ct = default)
        {
            refreshToken.RevokedAt = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            await context.SaveChangesAsync();

        }
        public Task RevokeAllUserTokensAsync(Guid userId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        private async Task RevokeAllRefreshTokensAsync(Guid userId, string userAgent, string ipAddress)
        {
            var tokens = await context.RefreshTokens
                .Where(t => t.UserId == userId
                    && t.UserAgent == userAgent
                    && t.RevokedAt == null
                ).ToListAsync();

            foreach (var token in tokens)
            {
                token.RevokedAt = DateTime.UtcNow;
                token.RevokedByIp = ipAddress;
            }

            await context.SaveChangesAsync();
        }


    }
}
