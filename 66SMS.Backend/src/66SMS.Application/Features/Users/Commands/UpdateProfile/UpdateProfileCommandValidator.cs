using FluentValidation;

namespace _66SMS.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(50).WithMessage("Full name cannot exceed 50 characters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?[0-9]{10,15}$").WithMessage("Invalid phone number format.");

            RuleFor(x => x.ProfilePhotoUrl)
                .Must(uri => uri == null || Uri.TryCreate(uri, UriKind.Absolute, out _))
                .WithMessage("Profile photo URL must be a valid URL.");
        }
    }
}
