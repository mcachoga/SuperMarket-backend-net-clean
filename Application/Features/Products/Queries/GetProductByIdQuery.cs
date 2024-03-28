using AutoMapper;
using MediatR;
using SuperMarket.Application.Services;
using SuperMarket.Common.Responses.Products;
using SuperMarket.Common.Responses.Wrappers;

namespace SuperMarket.Application.Features.Products.Queries
{
    public class GetProductByIdQuery : IRequest<IResponseWrapper>
    {
        public int ProductId { get; set; }
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, IResponseWrapper>
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IProductService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var currentEntity = await _service.GetProductByIdAsync(request.ProductId);
            if (currentEntity is not null)
            {
                var model = _mapper.Map<ProductResponse>(currentEntity);
                return await ResponseWrapper<ProductResponse>.SuccessAsync(model);
            }
            return await ResponseWrapper.FailAsync("Product does not exist.");
        }
    }
}