using MediatR;
using SuperMarket.Application.Services;
using SuperMarket.Common.Responses.Wrappers;

namespace SuperMarket.Application.Features.Orders.Commands
{
    public class DeleteOrderCommand : IRequest<IResponseWrapper>
    {
        public int OrderId { get; set; }
    }

    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, IResponseWrapper>
    {
        private readonly IOrderService _service;

        public DeleteOrderCommandHandler(IOrderService service)
        {
            _service = service;
        }

        public async Task<IResponseWrapper> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var currentEntity = await _service.GetOrderByIdAsync(request.OrderId);
            
            if (currentEntity is not null)
            {
                var marketId = await _service.DeleteOrderAsync(currentEntity);
                return await ResponseWrapper<int>.SuccessAsync(marketId, "Order entry deleted successfully.");
            }
            else
            {
                return await ResponseWrapper.FailAsync("Order does not exist.");
            }
        }
    }
}