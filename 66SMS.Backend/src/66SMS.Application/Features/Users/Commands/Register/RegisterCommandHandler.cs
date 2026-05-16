using _66SMS.Application.Abstractions.Services;
using _66SMS.Application.DTOs.Identity.Commands;
using _66SMS.Domain.Abstractions.Repositories.Sql;
using _66SMS.Domain.Entities;
using MediatR;

using _66SMS.Contracts.Shared;

namespace _66SMS.Application.Features.Users.Commands.Register
{
    public sealed record RegisterCommand(
        string UserName, 
        string Email, 
        string Password, 
        string? PhoneNumber, 
        string? FullName) : IRequest<Result<bool>>;

    public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<bool>>
    {
        private readonly IUserAuthRepository userAuthRepo;
        private readonly IUserSqlRepository userSqlRepo;
        private readonly IEmailService emailService;

        public RegisterCommandHandler(
            IUserAuthRepository userAuthRepo,
            IUserSqlRepository userSqlRepo,
            IEmailService emailService)
        {
            this.userAuthRepo = userAuthRepo;
            this.userSqlRepo = userSqlRepo;
            this.emailService = emailService;
        }

        public async Task<Result<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {

            // Kiểm tra email/username đã tồn tại chưa
            var existingByEmail = await userSqlRepo.FindByEmailAsync(request.Email, cancellationToken);
            if (existingByEmail != null) return Result<bool>.Conflict("Email already exists.");

            var existingByName = await userSqlRepo.FindByUserNameAsync(request.UserName, cancellationToken);
            if (existingByName != null) return Result<bool>.Conflict("Username already exists.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                Email = request.Email,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                IsActive = true,
                IsEmailConfirmed = false,
                CreatedAt = DateTime.UtcNow
            };

            var created = await userAuthRepo.CreateUserAsync(user, request.Password, cancellationToken);
            if (!created) return Result<bool>.BadRequest("Failed to create user.");

            // Thêm role mặc định
            await userAuthRepo.AddUserToRoleAsync(user, "Admin", cancellationToken);

            // Gửi email xác nhận (fire-and-forget safe)
            var token = await userAuthRepo.GenerateEmailConfirmationTokenAsync(user, cancellationToken);
            if (!string.IsNullOrEmpty(token))
            {
                // Link sẽ được frontend build từ userId + token
                var confirmLink = $"https://localhost:7777/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";
                await emailService.SendEmailConfirmationAsync(request.Email, request.UserName, confirmLink, cancellationToken);
            }

            return Result<bool>.Created(true, "Registration successful.");
        }
    }
}
