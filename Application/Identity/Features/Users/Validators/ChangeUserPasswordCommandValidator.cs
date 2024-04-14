using FluentValidation;
using SuperMarket.Application.Features.Identity.Users.Commands;

namespace SuperMarket.Application.Features.Identity.Users.Validators
{
    public class ChangeUserPasswordCommandValidator : AbstractValidator<ChangeUserPasswordCommand>
    {
        public ChangeUserPasswordCommandValidator()
        {
            RuleFor(command => command.ChangePassword).SetValidator(new ChangeUserPasswordRequestValidator());
        }
    }
}