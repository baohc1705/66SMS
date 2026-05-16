using _66SMS.Domain.Abstractions.Repositories.Sql;
using _66SMS.Domain.Entities;
using _66SMS.Persistence.Configurations.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace _66SMS.Persistence.Repositories.Sql
{
    public class UserAuthRepository : IUserAuthRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public UserAuthRepository(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<bool> CreateUserAsync(User user, string password, CancellationToken ct = default)
        {
            ApplicationUser? appUser = mapper.Map<ApplicationUser>(user);
            var result = await userManager.CreateAsync(appUser, password);
            return result.Succeeded;
        }

        public async Task<bool> UpdateUserAsync(User user, CancellationToken ct = default)
        {
            var appUser = await userManager.FindByIdAsync(user.Id.ToString());
            if (appUser == null) return false;

            appUser.UserName = user.UserName;
            appUser.Email = user.Email;
            appUser.FullName = user.FullName;
            appUser.PhoneNumber = user.PhoneNumber;
            appUser.ProfilePhotoUrl = user.ProfilePhotoUrl;
            appUser.DoB = user.DoB;

            appUser.ModifiedAt = DateTime.UtcNow;

            var result = await userManager.UpdateAsync(appUser);
            return result.Succeeded;
        }
        public async Task UpdateLastLoginAsync(User user, DateTime loginTime, CancellationToken ct = default)
        {
            var appUser = await userManager.FindByIdAsync(user.Id.ToString());
            if (appUser == null) return;
            appUser.LastLoginAt = loginTime;
            await userManager.UpdateAsync(appUser);
        }

        public async Task<bool> ChangePasswordAsync(User user, string currentPassword, string newPassword, CancellationToken ct = default)
        {
            var appUser = await userManager.FindByIdAsync(user.Id.ToString());
            if (appUser == null) return false;
            var result = await userManager.ChangePasswordAsync(appUser, currentPassword, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> CheckPasswordAsync(User user, string password, CancellationToken ct = default)
        {
            var appUser = await userManager.FindByIdAsync(user.Id.ToString());
            if (appUser == null) return false;
            return await userManager.CheckPasswordAsync(appUser, password);
        }

        public async Task<string?> GeneratePasswordResetTokenAsync(User user, CancellationToken ct = default)
        {
            var appUser = await userManager.FindByIdAsync(user.Id.ToString());
            if (appUser is null) return null;
            var token = await userManager.GeneratePasswordResetTokenAsync(appUser);
            return token;

        }
        public async Task<bool> ResetPasswordAsync(User user, string token, string newPassword, CancellationToken ct = default)
        {
            var appUser = await userManager.FindByIdAsync(user.Id.ToString());
            if (appUser == null) return false;
            var result = await userManager.ResetPasswordAsync(appUser, token, newPassword);
            return result.Succeeded;
        }
        public async Task<string?> GenerateEmailConfirmationTokenAsync(User user, CancellationToken ct = default)
        {
            var appUser = await userManager.FindByIdAsync(user.Id.ToString());
            if (appUser is null) return null;
            return await userManager.GenerateEmailConfirmationTokenAsync(appUser);

        }

        public async Task<bool> VerifyConfirmationEmailAsync(User user, string token, CancellationToken ct = default)
        {
            var appUser = await userManager.FindByIdAsync(user.Id.ToString());
            if (appUser == null) return false;
            var result = await userManager.ConfirmEmailAsync(appUser, token);
            return result.Succeeded;
        }

        public async Task<IList<string>> GetUserRolesAsync(User user, CancellationToken ct = default)
        {
            var appUser = await userManager.FindByIdAsync(user.Id.ToString());
            if (appUser == null) return new List<string>();
            return await userManager.GetRolesAsync(appUser);

        }
        public async Task<bool> AddUserToRoleAsync(User user, string role, CancellationToken ct = default)
        {
            var appUser = await userManager.FindByIdAsync(user.Id.ToString());
            if (appUser == null) return false;
            var result = await userManager.AddToRoleAsync(appUser, role);
            return result.Succeeded;

        }
        public async Task IncrementAccessFailedCountAsync(User user, CancellationToken ct = default)
        {
            var appUser = await userManager.FindByIdAsync(user.Id.ToString());
            if (appUser != null)
                await userManager.AccessFailedAsync(appUser);
        }

        public async Task ResetAccessFailedCountAsync(User user, CancellationToken ct = default)
        {
            var appUser = await userManager.FindByIdAsync(user.Id.ToString());
            if (appUser != null)
                await userManager.ResetAccessFailedCountAsync(appUser);
        }
    }
}
