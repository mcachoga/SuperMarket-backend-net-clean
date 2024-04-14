using FluentValidation;
using SuperMarket.Shared.Requests.Identity;

namespace SuperMarket.Application.Features.Identity.Users.Validators
{
    public class ChangeUserPasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangeUserPasswordRequestValidator()
        {
            RuleFor(request => request.CurrentPassword)
                .NotEmpty()
                    .WithMessage("{PropertyName} is required.");

            RuleFor(request => request.NewPassword)
                .NotEmpty()
                    .WithMessage("{PropertyName} is required.")
                .Must((req, current) => req.CurrentPassword != current)
                .WithMessage("Password should be different.");

            RuleFor(request => request.ConfirmedNewPassword)
                .NotEmpty()
                    .WithMessage("{PropertyName} is required.")
                .Must( (req, confirmed) => req.NewPassword == confirmed )
                    .WithMessage("Passwords do not match.");
        }
    }
}