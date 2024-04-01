using AutoMapper;
using MediatR;
using SuperMarket.Application.Services;
using SuperMarket.Common.Responses.Markets;
using SuperMarket.Common.Responses.Wrappers;

namespace SuperMarket.Application.Features.Employees.Queries
{
    public class GetMarketByIdQuery : IRequest<IResponseWrapper>
    {
        public int MarketId { get; set; }
    }

    public class GetMarketByIdQueryHandler : IRequestHandler<GetMarketByIdQuery, IResponseWrapper>
    {
        private readonly IMarketRepository _repository;
        private readonly IMapper _mapper;

        public GetMarketByIdQueryHandler(IMarketRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(GetMarketByIdQuery request, CancellationToken cancellationToken)
        {
            var currentEntity = await _repository.GetByIdAsync(request.MarketId);

            if (currentEntity is not null)
            {
                var model = _mapper.Map<MarketResponse>(currentEntity);
                return await ResponseWrapper<MarketResponse>.SuccessAsync(model);
            }

            return await ResponseWrapper.FailAsync("Market does not exist.");
        }
    }
}