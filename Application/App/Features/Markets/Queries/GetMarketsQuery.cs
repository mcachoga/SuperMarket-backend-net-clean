using AutoMapper;
using MediatR;
using SuperMarket.Application.Services.Contracts;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Shared.Responses.Catalog;

namespace SuperMarket.Application.Features.Markets.Queries
{
    public class GetMarketsQuery : IRequest<IResponseWrapper>
    {

    }

    public class GetMarketsQueryHandler : IRequestHandler<GetMarketsQuery, IResponseWrapper>
    {
        private readonly IMarketRepository _repository;
        private readonly IMapper _mapper;

        public GetMarketsQueryHandler(IMarketRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(GetMarketsQuery request, CancellationToken cancellationToken)
        {
            var entitiesList = await _repository.GetListAsync();

            if (entitiesList.Count > 0)
            {
                var modelsList = _mapper.Map<List<MarketResponse>>(entitiesList);
                return await ResponseWrapper<List<MarketResponse>>.SuccessAsync(modelsList);
            }

            return await ResponseWrapper.FailAsync("No markets were found.");
        }
    }
}