using FluentValidation;
using SuperMarket.Application.Services.Contracts;
using SuperMarket.Domain;
using SuperMarket.Shared.Requests.Catalog;

namespace SuperMarket.Application.Features.Products.Validators
{
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator(IProductRepository repository)
        {
            RuleFor(request => request.Id)
                .MustAsync(async (id, ct) => 
                    await repository.GetByIdAsync(id) is Product currentMarket && currentMarket.Id == id)
                .WithMessage("Product does not exist.");

            RuleFor(request => request.Name)
                .NotEmpty()
                    .WithMessage("{PropertyName} is required.")
                .MaximumLength(60)
                    .WithMessage("{PropertyName} can not be greater than {MaxLength} characters long.");
        }
    }
}