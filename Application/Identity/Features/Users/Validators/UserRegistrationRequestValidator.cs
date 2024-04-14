using FluentValidation;
using SuperMarket.Application.Identity.Services.Contracts;
using SuperMarket.Domain.Identity;
using SuperMarket.Shared.Requests.Identity;

namespace SuperMarket.Application.Features.Identity.Users.Validators
{
    public class UserRegistrationRequestValidator : AbstractValidator<UserRegistrationRequest>
    {
        public UserRegistrationRequestValidator(IUserService userService)
        {
            RuleFor(request => request.Email)
                .NotEmpty()
                    .WithMessage("{PropertyName} is required.")
                .EmailAddress()
                    .WithMessage("{PropertyName} is not a valid email.")
                .MaximumLength(256)
                    .WithMessage("Long of {PropertyName} should be shorter than {MaxValue}")
                .MustAsync(async (email, ct) =>  await userService.GetUserByEmailAsync(email) is not ApplicationUser existingUser)
                    .WithMessage("Email already taken.");

            RuleFor(request => request.FirstName)
                .NotEmpty()
                    .WithMessage("{PropertyName} is required.")
                .MaximumLength(60)
                    .WithMessage("Long of {PropertyName} should be shorter than {MaxValue}");

            RuleFor(request => request.LastName)
                .NotEmpty()
                    .WithMessage("{PropertyName} is required.")
                .MaximumLength(60)
                    .WithMessage("Long of {PropertyName} should be shorter than {MaxValue}");

            RuleFor(request => request.UserName)
                .NotEmpty()
                    .WithMessage("{PropertyName} is required.")
                .MaximumLength(256)
                    .WithMessage("Long of {PropertyName} should be shorter than {MaxValue}")
                .MustAsync(async (name, ct) => await userService.GetUserByNameAsync(name) is not ApplicationUser existingUser)
                    .WithMessage("Name already taken.");

            RuleFor(request => request.Password)
                .NotEmpty()
                    .WithMessage("{PropertyName} is required.");

            RuleFor(request => request.ComfirmPassword)
                .NotEmpty()
                    .WithMessage("{PropertyName} is required.")
                .Must( (req, confirmed) => req.Password == confirmed )
                    .WithMessage("Passwords do not match.");
        }
    }
}