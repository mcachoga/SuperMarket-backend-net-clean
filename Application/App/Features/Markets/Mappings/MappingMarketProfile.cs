using AutoMapper;
using SuperMarket.Domain;
using SuperMarket.Shared.Requests.Catalog;
using SuperMarket.Shared.Responses.Catalog;

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