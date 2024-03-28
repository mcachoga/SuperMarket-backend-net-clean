using FluentValidation;
using SuperMarket.Application.Services;
using SuperMarket.Common.Requests.Products;
using SuperMarket.Domain;

namespace SuperMarket.Application.Features.Products.Validators
{
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator(IProductService service)
        {
            RuleFor(request => request.Id)
                .MustAsync(async (id, ct) => 
                    await service.GetProductByIdAsync(id) is Product currentMarket && currentMarket.Id == id)
                .WithMessage("Product does not exist.");

            RuleFor(request => request.Name)
                .NotEmpty()
                    .WithMessage("{PropertyName} is required.")
                .MaximumLength(60)
                    .WithMessage("{PropertyName} can not be greater than {MaxLength} characters long.");
        }
    }
}