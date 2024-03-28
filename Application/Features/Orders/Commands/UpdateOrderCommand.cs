using AutoMapper;
using MediatR;
using SuperMarket.Application.Services;
using SuperMarket.Application.Validations.Pipelines;
using SuperMarket.Common.Requests.Orders;
using SuperMarket.Common.Responses.Orders;
using SuperMarket.Common.Responses.Products;
using SuperMarket.Common.Responses.Wrappers;

namespace SuperMarket.Application.Features.Orders.Commands
{
    public class UpdateOrderCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public UpdateOrderRequest UpdateRequest { get; set; }
    }

    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, IResponseWrapper>
    {
        private readonly IOrderService _service;
        private readonly IMapper _mapper;

        public UpdateOrderCommandHandler(IOrderService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var currentEntity = await _service.GetOrderByIdAsync(request.UpdateRequest.Id);

            if (currentEntity is not null)
            {
                currentEntity.OrderDate = request.UpdateRequest.OrderDate;
                currentEntity.Price = request.UpdateRequest.Price;
                currentEntity.MarketId = request.UpdateRequest.MarketId;
                currentEntity.ProductId = request.UpdateRequest.ProductId;
                
                var updatedEntity = await _service.UpdateOrderAsync(currentEntity);
                var model = _mapper.Map<OrderResponse>(updatedEntity);

                return await ResponseWrapper<OrderResponse>.SuccessAsync(model, "Order updated successfully.");
            }

            return await ResponseWrapper<ProductResponse>.FailAsync("Order does not exist.");
        }
    }
}