using AutoMapper;
using MediatR;
using SuperMarket.Application.Services;
using SuperMarket.Common.Responses.Orders;
using SuperMarket.Common.Responses.Wrappers;

namespace SuperMarket.Application.Features.Orders.Queries
{
    public class GetOrdersQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IResponseWrapper>
    {
        private readonly IOrderService _service;
        private readonly IMapper _mapper;

        public GetOrdersQueryHandler(IOrderService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var entitiesList = await _service.GetOrderListAsync();
            if (entitiesList.Count > 0)
            {
                var modelsList = _mapper.Map<List<OrderResponse>>(entitiesList);
                return await ResponseWrapper<List<OrderResponse>>.SuccessAsync(modelsList);
            }

            return await ResponseWrapper.FailAsync("No orders were found.");
        }
    }
}