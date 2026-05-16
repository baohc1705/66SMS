using FluentValidation;

namespace _66SMS.Application.Features.Users.Commands.AddOrUpdateAddress
{
    public class AddOrUpdateAddressCommandValidator : AbstractValidator<AddOrUpdateAddressCommand>
    {
        public AddOrUpdateAddressCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(50).WithMessage("City cannot exceed 50 characters.");

            RuleFor(x => x.Ward)
                .NotEmpty().WithMessage("Ward is required.")
                .MaximumLength(50).WithMessage("Ward cannot exceed 50 characters.");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street is required.")
                .MaximumLength(100).WithMessage("Street cannot exceed 100 characters.");
        }
    }
}
