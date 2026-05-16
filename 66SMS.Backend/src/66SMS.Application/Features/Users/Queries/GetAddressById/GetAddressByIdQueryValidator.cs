using FluentValidation;

namespace _66SMS.Application.Features.Users.Queries.GetAddressById
{
    public class GetAddressByIdQueryValidator : AbstractValidator<GetAddressByIdQuery>
    {
        public GetAddressByIdQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x => x.AddressId)
                .NotEmpty().WithMessage("Address ID is required.");
        }
    }
}
