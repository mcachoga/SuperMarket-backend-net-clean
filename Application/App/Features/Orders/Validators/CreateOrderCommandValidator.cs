using FluentValidation;
using SuperMarket.Application.Features.Orders.Commands;
using SuperMarket.Application.Services.Contracts;

namespace SuperMarket.Application.Features.Orders.Validators
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(command => command.CreateRequest).SetValidator(new CreateOrderRequestValidator(unitOfWork));
        }
    }
}