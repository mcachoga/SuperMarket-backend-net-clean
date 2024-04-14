using FluentValidation;
using SuperMarket.Application.Features.Products.Commands;
using SuperMarket.Application.Services.Contracts;

namespace SuperMarket.Application.Features.Products.Validators
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator(IProductRepository repository)
        {
            RuleFor(command => command.UpdateRequest).SetValidator(new UpdateProductRequestValidator(repository));
        }
    }
}