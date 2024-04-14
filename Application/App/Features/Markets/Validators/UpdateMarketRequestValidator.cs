using FluentValidation;
using SuperMarket.Application.Services.Contracts;
using SuperMarket.Domain;
using SuperMarket.Shared.Requests.Catalog;

namespace SuperMarket.Application.Features.Markets.Validators
{
    public class UpdateMarketRequestValidator : AbstractValidator<UpdateMarketRequest>
    {
        public UpdateMarketRequestValidator(IMarketRepository repository)
        {
            RuleFor(request => request.Id)
                .MustAsync(async (id, ct) => await repository.GetByIdAsync(id) is Market currentMarket && currentMarket.Id == id)
                    .WithMessage("Market does not exist.");

            RuleFor(request => request.Name)
                .NotEmpty()
                    .WithMessage("{PropertyName} is required.")
                .MaximumLength(60)
                    .WithMessage("{PropertyName} can not be greater than {MaxLength} characters long.");
        }
    }
}