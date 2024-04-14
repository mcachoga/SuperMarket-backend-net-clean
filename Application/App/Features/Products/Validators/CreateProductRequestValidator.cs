using FluentValidation;
using SuperMarket.Shared.Requests.Catalog;

namespace SuperMarket.Application.Features.Products.Validators
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty()
                    .WithMessage("{PropertyName} is required.")
                .MaximumLength(60)
                    .WithMessage("{PropertyName} can not be greater than {MaxLength} characters long."); ;
        }
    }
}