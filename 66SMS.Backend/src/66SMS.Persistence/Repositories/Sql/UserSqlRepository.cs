using _66SMS.Domain.Abstractions.Repositories.Sql;
using _66SMS.Domain.Entities;
using _66SMS.Persistence.Configurations.Identity;
using _66SMS.Persistence.Repositories.Sql.Base;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace _66SMS.Persistence.Repositories.Sql
{
    public class UserSqlRepository : IUserSqlRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        public UserSqlRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }
        public async Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            ApplicationUser? appUser = await userManager.FindByIdAsync(id.ToString());
            return mapper.Map<User?>(appUser) ?? null;
        }
        public async Task<User?> FindByEmailAsync(string email, CancellationToken ct = default)
        {
            var appUser = await userManager.FindByEmailAsync(email);
            if (appUser == null)
                return null;

            return mapper.Map<User>(appUser);

        }

        public async Task<User?> FindByUserNameAsync(string userName, CancellationToken ct = default)
        {
            var appUser = await userManager.FindByNameAsync(userName);
            if (appUser == null)
                return null;

            return mapper.Map<User>(appUser);

        }

        public async Task<int> GetAccessFailedCountAsync(User user, CancellationToken ct = default)
        {
            ApplicationUser? appUser = await userManager.FindByIdAsync(user.Id.ToString());
            return appUser?.AccessFailedCount ?? 0;
        }

        public async Task<DateTime?> GetLockoutEndDateAsync(User user, CancellationToken ct = default)
        {
            var appUser = await userManager.FindByIdAsync(user.Id.ToString());
            if (appUser is null) return null;
            return appUser.LockoutEnd?.UtcDateTime;
        }

        public Task<int> GetMaxFailedAccessAttemptsAsync(CancellationToken ct = default)
        {
            return Task.FromResult(userManager.Options.Lockout.MaxFailedAccessAttempts);
        }

        public async Task<bool> IsLockedOutAsync(User user, CancellationToken ct = default)
        {
            var appUser = await userManager.FindByIdAsync(user.Id.ToString());
            return appUser != null && await userManager.IsLockedOutAsync(appUser);
        }

        public async Task<bool> IsTwoFactorEnabledAsync(User user, CancellationToken ct = default)
        {
            var appUser = await userManager.FindByIdAsync(user.Id.ToString());
            return appUser != null && await userManager.GetTwoFactorEnabledAsync(appUser);
        }

        public async Task<bool> IsUserExistsAsync(Guid userId, CancellationToken ct = default)
        {
            var appUser = await userManager.FindByIdAsync(userId.ToString());
            return appUser != null ? true : false;
        }
    }
}
