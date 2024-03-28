using FluentValidation;
using SuperMarket.Application.Features.Identity.Users.Commands;
using SuperMarket.Application.Services.Identity;

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