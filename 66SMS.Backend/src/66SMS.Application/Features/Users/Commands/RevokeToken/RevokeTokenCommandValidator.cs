using FluentValidation;

namespace _66SMS.Application.Features.Users.Commands.RevokeToken
{
    public class RevokeTokenCommandValidator : AbstractValidator<RevokeTokenCommand>
    {
        public RevokeTokenCommandValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.");
        }
    }
}
