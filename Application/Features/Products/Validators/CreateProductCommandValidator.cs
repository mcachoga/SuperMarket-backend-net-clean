using FluentValidation;
using SuperMarket.Application.Features.Products.Commands;

namespace SuperMarket.Application.Features.Products.Validators
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(command => command.CreateRequest).SetValidator(new CreateProductRequestValidator());
        }
    }
}