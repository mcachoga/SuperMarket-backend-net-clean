using FluentValidation;
using SuperMarket.Application.Features.Markets.Commands;
using SuperMarket.Application.Services.Contracts;

namespace SuperMarket.Application.Features.Markets.Validators
{
    public class UpdateMarketCommandValidator : AbstractValidator<UpdateMarketCommand>
    {
        public UpdateMarketCommandValidator(IMarketRepository repository)
        {
            RuleFor(command => command.UpdateRequest).SetValidator(new UpdateMarketRequestValidator(repository));
        }
    }
}