using FluentValidation;
using SuperMarket.Application.Features.Identity.Roles.Commands;

namespace SuperMarket.Application.Features.Markets.Validators
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(command => command.CreateRole).SetValidator(new CreateRoleRequestValidator());
        }
    }
}