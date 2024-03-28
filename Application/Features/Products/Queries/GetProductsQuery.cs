using AutoMapper;
using MediatR;
using SuperMarket.Application.Services;
using SuperMarket.Common.Responses.Products;
using SuperMarket.Common.Responses.Wrappers;

namespace SuperMarket.Application.Features.Products.Queries
{
    public class GetProductsQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IResponseWrapper>
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(IProductService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var entitiesList = await _service.GetProductListAsync();
            if (entitiesList.Count > 0)
            {
                var modelsList = _mapper.Map<List<ProductResponse>>(entitiesList);
                return await ResponseWrapper<List<ProductResponse>>.SuccessAsync(modelsList);
            }
            return await ResponseWrapper.FailAsync("No products were found.");
        }
    }
}