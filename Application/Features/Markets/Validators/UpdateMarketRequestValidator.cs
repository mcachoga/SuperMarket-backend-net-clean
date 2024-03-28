using FluentValidation;
using SuperMarket.Application.Services;
using SuperMarket.Common.Requests.Markets;
using SuperMarket.Domain;

namespace SuperMarket.Application.Features.Markets.Validators
{
    public class UpdateMarketRequestValidator : AbstractValidator<UpdateMarketRequest>
    {
        public UpdateMarketRequestValidator(IMarketService marketService)
        {
            RuleFor(request => request.Id)
                .MustAsync(async (id, ct) => await marketService.GetMarketByIdAsync(id) is Market currentMarket && currentMarket.Id == id)
                    .WithMessage("Market does not exist.");

            RuleFor(request => request.Name)
                .NotEmpty()
                    .WithMessage("{PropertyName} is required.")
                .MaximumLength(60)
                    .WithMessage("{PropertyName} can not be greater than {MaxLength} characters long.");
        }
    }
}