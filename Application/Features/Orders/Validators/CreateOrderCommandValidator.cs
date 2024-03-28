using FluentValidation;
using SuperMarket.Application.Features.Orders.Commands;
using SuperMarket.Application.Services;

namespace SuperMarket.Application.Features.Orders.Validators
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator(IOrderService orderService, IProductService productService, IMarketService marketService)
        {
            RuleFor(command => command.CreateRequest).SetValidator(new CreateOrderRequestValidator(orderService, productService, marketService));
        }
    }
}