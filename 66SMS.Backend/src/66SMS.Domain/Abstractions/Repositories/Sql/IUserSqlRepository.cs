using _66SMS.Domain.Abstractions.Repositories.Sql.Base;
using _66SMS.Domain.Entities;

namespace _66SMS.Domain.Abstractions.Repositories.Sql
{
    public interface IUserSqlRepository 
    {
        Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<User?> FindByEmailAsync(string email, CancellationToken ct = default);
        Task<User?> FindByUserNameAsync(string userName, CancellationToken ct = default);
        Task<bool> IsUserExistsAsync(Guid userId, CancellationToken ct = default);
        Task<bool> IsLockedOutAsync(User user, CancellationToken ct = default);
        Task<bool> IsTwoFactorEnabledAsync(User user, CancellationToken ct = default);
        Task<DateTime?> GetLockoutEndDateAsync(User user, CancellationToken ct = default);
        Task<int> GetAccessFailedCountAsync(User user, CancellationToken ct = default);
        Task<int> GetMaxFailedAccessAttemptsAsync(CancellationToken ct = default);
    }
}
