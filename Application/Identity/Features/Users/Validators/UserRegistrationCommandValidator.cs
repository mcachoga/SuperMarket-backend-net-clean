using FluentValidation;
using SuperMarket.Application.Features.Identity.Users.Commands;
using SuperMarket.Application.Identity.Services.Contracts;

namespace SuperMarket.Application.Features.Identity.Users.Validators
{
    public class UserRegistrationCommandValidator : AbstractValidator<UserRegistrationCommand>
    {
        public UserRegistrationCommandValidator(IUserService userService)
        {
            RuleFor(command => command.UserRegistration).SetValidator(new UserRegistrationRequestValidator(userService));
        }
    }
}