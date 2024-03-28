using FluentValidation;
using SuperMarket.Application.Features.Markets.Commands;

namespace SuperMarket.Application.Features.Markets.Validators
{
    public class CreateMarketCommandValidator : AbstractValidator<CreateMarketCommand>
    {
        public CreateMarketCommandValidator()
        {
            RuleFor(command => command.CreateRequest).SetValidator(new CreateMarketRequestValidator());
        }
    }
}