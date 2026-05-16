using FluentValidation;

namespace _66SMS.Application.Features.Users.Commands.DeleteAddress
{
    public class DeleteAddressCommandValidator : AbstractValidator<DeleteAddressCommand>
    {
        public DeleteAddressCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x => x.AddressId)
                .NotEmpty().WithMessage("Address ID is required.");
        }
    }
}
