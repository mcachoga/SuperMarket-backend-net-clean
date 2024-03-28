using AutoMapper;
using SuperMarket.Common.Requests.Markets;
using SuperMarket.Common.Responses.Markets;
using SuperMarket.Domain;

namespace SuperMarket.Application.Features.Markets.Mappings
{
    public class MappingMarketProfile : Profile
    {
        public MappingMarketProfile()
        {
            CreateMap<CreateMarketRequest, Market>();
            CreateMap<Market, MarketResponse>();
        }
    }
}