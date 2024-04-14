using AutoMapper;
using SuperMarket.Domain;
using SuperMarket.Shared.Requests.Catalog;
using SuperMarket.Shared.Responses.Catalog;

namespace SuperMarket.Application.Features.Orders.Mappings
{
    public class MappingOrderProfile : Profile
    {
        public MappingOrderProfile()
        {
            CreateMap<CreateOrderRequest, Order>();
            CreateMap<Order, OrderResponse>();
        }
    }
}