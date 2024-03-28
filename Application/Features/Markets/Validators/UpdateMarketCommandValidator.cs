using FluentValidation;
using SuperMarket.Application.Features.Markets.Commands;
using SuperMarket.Application.Services;

namespace SuperMarket.Application.Features.Markets.Validators
{
    public class UpdateMarketCommandValidator : AbstractValidator<UpdateMarketCommand>
    {
        public UpdateMarketCommandValidator(IMarketService service)
        {
            RuleFor(command => command.UpdateRequest).SetValidator(new UpdateMarketRequestValidator(service));
        }
    }
}