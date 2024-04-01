using AutoMapper;
using MediatR;
using SuperMarket.Application.Services;
using SuperMarket.Common.Responses.Orders;
using SuperMarket.Common.Responses.Wrappers;

namespace SuperMarket.Application.Features.Orders.Queries
{
    public class GetOrderByIdQuery : IRequest<IResponseWrapper>
    {
        public int OrderId { get; set; }
    }

    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, IResponseWrapper>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public GetOrderByIdQueryHandler(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var currentEntity = await _repository.GetByIdAsync(request.OrderId);
            
            if (currentEntity is not null)
            {
                var model = _mapper.Map<OrderResponse>(currentEntity);
                return await ResponseWrapper<OrderResponse>.SuccessAsync(model);
            }

            return await ResponseWrapper.FailAsync("Order does not exist.");
        }
    }
}