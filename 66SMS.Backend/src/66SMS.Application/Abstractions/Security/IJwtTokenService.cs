using _66SMS.Domain.Entities;

namespace _66SMS.Application.Abstractions.Security
{
    /// <summary>
    /// Abstraction cho JWT token generation.
    /// Interface được đặt trong Application để giữ đúng chiều dependency:
    /// Infrastructure implements → Application (không ngược lại).
    /// </summary>
    public interface IJwtTokenService
    {
        /// <summary>
        /// Generates a signed JWT access token for the given user.
        /// </summary>
        string GenerateAccessToken(User user, IList<string> roles);
    }
}
