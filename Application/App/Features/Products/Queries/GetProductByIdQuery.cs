using AutoMapper;
using MediatR;
using SuperMarket.Application.Services.Contracts;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Shared.Responses.Catalog;

namespace SuperMarket.Application.Features.Products.Queries
{
    public class GetProductByIdQuery : IRequest<IResponseWrapper>
    {
        public int ProductId { get; set; }
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, IResponseWrapper>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var currentEntity = await _repository.GetByIdAsync(request.ProductId);
            
            if (currentEntity is not null)
            {
                var model = _mapper.Map<ProductResponse>(currentEntity);
                return await ResponseWrapper<ProductResponse>.SuccessAsync(model);
            }
            
            return await ResponseWrapper.FailAsync("Product does not exist.");
        }
    }
}