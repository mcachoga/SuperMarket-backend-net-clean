using AutoMapper;
using SuperMarket.Common.Requests.Orders;
using SuperMarket.Common.Responses.Orders;
using SuperMarket.Domain;

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