using _66SMS.Domain.Entities;

namespace _66SMS.Domain.Abstractions.Repositories.Sql
{
    public interface IUserAuthRepository
    {
        Task<bool> CreateUserAsync(User user, string password, CancellationToken ct = default);
        Task<bool> UpdateUserAsync(User user, CancellationToken ct = default);
        Task UpdateLastLoginAsync(User user, DateTime loginTime, CancellationToken ct = default);

 
        Task<bool> CheckPasswordAsync(User user, string password, CancellationToken ct = default);
        Task<bool> ChangePasswordAsync(User user, string currentPassword, string newPassword, CancellationToken ct = default);
        Task<string?> GeneratePasswordResetTokenAsync(User user, CancellationToken ct = default);
        Task<bool> ResetPasswordAsync(User user, string token, string newPassword, CancellationToken ct = default);

        Task<string?> GenerateEmailConfirmationTokenAsync(User user, CancellationToken ct = default);
        Task<bool> VerifyConfirmationEmailAsync(User user, string token, CancellationToken ct = default);

        Task<IList<string>> GetUserRolesAsync(User user, CancellationToken ct = default);
        Task<bool> AddUserToRoleAsync(User user, string role, CancellationToken ct = default);

        Task IncrementAccessFailedCountAsync(User user, CancellationToken ct = default);
        Task ResetAccessFailedCountAsync(User user, CancellationToken ct = default);
    }
}
