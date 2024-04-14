using AutoMapper;
using SuperMarket.Domain;
using SuperMarket.Shared.Requests.Catalog;
using SuperMarket.Shared.Responses.Catalog;

namespace SuperMarket.Application.Features.Products.Mappings
{
    public class MappingProductProfile : Profile
    {
        public MappingProductProfile()
        {
            CreateMap<CreateProductRequest, Product>();
            CreateMap<Product, ProductResponse>();
        }
    }
}