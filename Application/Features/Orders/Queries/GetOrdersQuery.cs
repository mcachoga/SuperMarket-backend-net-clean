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
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public GetOrdersQueryHandler(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var entitiesList = await _repository.GetListAsync();

            if (entitiesList.Count > 0)
            {
                var modelsList = _mapper.Map<List<OrderResponse>>(entitiesList);
                return await ResponseWrapper<List<OrderResponse>>.SuccessAsync(modelsList);
            }

            return await ResponseWrapper.FailAsync("No orders were found.");
        }
    }
}