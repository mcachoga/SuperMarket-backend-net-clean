using AutoMapper;
using MediatR;
using SuperMarket.Application.Services.Contracts;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Shared.Responses.Catalog;

namespace SuperMarket.Application.Features.Products.Queries
{
    public class GetProductsQuery : IRequest<IResponseWrapper>
    {

    }

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IResponseWrapper>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var entitiesList = await _repository.GetListAsync();

            if (entitiesList.Count > 0)
            {
                var modelsList = _mapper.Map<List<ProductResponse>>(entitiesList);
                return await ResponseWrapper<List<ProductResponse>>.SuccessAsync(modelsList);
            }

            return await ResponseWrapper.FailAsync("No products were found.");
        }
    }
}