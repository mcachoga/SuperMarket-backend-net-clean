using FluentValidation;
using SuperMarket.Application.Services;
using SuperMarket.Application.Validations.Rules;
using SuperMarket.Common.Requests.Orders;
using SuperMarket.Domain;

namespace SuperMarket.Application.Features.Orders.Validators
{
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator(IOrderService orderService, IProductService productService, IMarketService marketService)
        {
            RuleFor(request => request.MarketId)
                .NotEmpty()
                    .WithMessage("Market is required.")
                .GreaterThan(0)
                    .WithMessage("{PropertyName} should be greater than 0.")
                .MustAsync(async (id, ct) =>
                    await marketService.GetMarketByIdAsync(id) is Market currentMarket && currentMarket.Id == id)
                    .WithMessage("Market does not exist.");

            RuleFor(request => request.ProductId)
                .NotEmpty()
                    .WithMessage("Product is required.")
                .GreaterThan(0)
                    .WithMessage("{PropertyName} should be greater than 0.")
                .MustAsync(async (id, ct) =>
                    await productService.GetProductByIdAsync(id) is Product currentProduct && currentProduct.Id == id)
                    .WithMessage("Product does not exist.");

            RuleFor(request => request.Price)
                .NotEmpty()
                    .WithMessage("{PropertyName} is required.")
                .GreaterThan(0)
                    .WithMessage("{PropertyName} should be greater than 0.");

            RuleFor(request => request.OrderDate)
                .NotEmpty()
                    .WithMessage("{PropertyName} is required.")
                .CanNotBeFuture();

            // REGLA DE NEGOCIO
            RuleFor(x => x)
                .MustAsync(async (x, cancellation) =>
                {
                    if (x.MarketId == 0 || x.ProductId == 0 || x.Price == 0)
                        return true;

                    var orders = await orderService.GetOrderListAsync();
                    orders = orders.Where(q => q.ProductId == x.ProductId && q.MarketId != x.MarketId && q.Price < x.Price).ToList();

                    return !orders.Any();
                })
                .WithMessage("There are cheater prices of this product in other markets.");
        }
    }
}