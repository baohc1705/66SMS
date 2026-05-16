using FluentValidation;

namespace _66SMS.Application.Features.Users.Commands.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");
        }
    }
}
