using AutoMapper;
using SuperMarket.Common.Requests.Products;
using SuperMarket.Common.Responses.Products;
using SuperMarket.Domain;

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