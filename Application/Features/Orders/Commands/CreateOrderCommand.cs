using AutoMapper;
using MediatR;
using SuperMarket.Application.Services;
using SuperMarket.Application.Validations.Pipelines;
using SuperMarket.Common.Requests.Orders;
using SuperMarket.Common.Responses.Orders;
using SuperMarket.Common.Responses.Wrappers;
using SuperMarket.Domain;

namespace SuperMarket.Application.Features.Orders.Commands
{
    public class CreateOrderCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public CreateOrderRequest CreateRequest { get; set; }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, IResponseWrapper>
    {
        private readonly IOrderService _service;
        private readonly IMapper _mapper;

        public CreateOrderCommandHandler(IOrderService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var model = _mapper.Map<Order>(request.CreateRequest);
            var newEntity = await _service.CreateOrderAsync(model);

            if (newEntity.Id > 0)
            {
                var newModel = _mapper.Map<OrderResponse>(newEntity);
                return await ResponseWrapper<OrderResponse>.SuccessAsync(newModel, "Order created Successfully.");
            }

            return await ResponseWrapper.FailAsync("Failed to create order entry.");
        }
    }
}