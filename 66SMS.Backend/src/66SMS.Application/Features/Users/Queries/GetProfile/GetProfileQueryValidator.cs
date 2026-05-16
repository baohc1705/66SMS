using FluentValidation;

namespace _66SMS.Application.Features.Users.Queries.GetProfile
{
    public class GetProfileQueryValidator : AbstractValidator<GetProfileQuery>
    {
        public GetProfileQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}
