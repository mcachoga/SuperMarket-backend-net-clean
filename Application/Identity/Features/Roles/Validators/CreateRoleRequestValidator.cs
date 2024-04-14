using FluentValidation;
using SuperMarket.Shared.Requests.Identity;

namespace SuperMarket.Application.Features.Markets.Validators
{
    public class CreateRoleRequestValidator : AbstractValidator<CreateRoleRequest>
    {
        public CreateRoleRequestValidator()
        {
            RuleFor(request => request.RoleName)
                .NotEmpty()
                    .WithMessage("{PropertyName} is required.")
                .MaximumLength(60)
                    .WithMessage("{PropertyName} can not be greater than {MaxLength} characters long.");
        }
    }
}