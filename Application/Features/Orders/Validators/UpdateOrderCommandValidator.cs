using FluentValidation;
using SuperMarket.Application.Features.Orders.Commands;
using SuperMarket.Application.Services;

namespace SuperMarket.Application.Features.Orders.Validators
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator(IOrderService orderService, IProductService productService, IMarketService marketService)
        {
            RuleFor(command => command.UpdateRequest).SetValidator(new UpdateOrderRequestValidator(orderService, productService, marketService));
        }
    }
}