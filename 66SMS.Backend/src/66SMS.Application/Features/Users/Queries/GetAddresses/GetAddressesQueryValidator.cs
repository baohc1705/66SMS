using FluentValidation;

namespace _66SMS.Application.Features.Users.Queries.GetAddresses
{
    public class GetAddressesQueryValidator : AbstractValidator<GetAddressesQuery>
    {
        public GetAddressesQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}
