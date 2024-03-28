using AutoMapper;
using MediatR;
using SuperMarket.Application.Services;
using SuperMarket.Application.Validations.Pipelines;
using SuperMarket.Common.Requests.Products;
using SuperMarket.Common.Responses.Products;
using SuperMarket.Common.Responses.Wrappers;
using SuperMarket.Domain;

namespace SuperMarket.Application.Features.Products.Commands
{
    public class CreateProductCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public CreateProductRequest CreateRequest { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, IResponseWrapper>
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(IProductService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var model = _mapper.Map<Product>(request.CreateRequest);
            var newEntity = await _service.CreateProductAsync(model);

            if (newEntity.Id > 0)
            {
                var newModel = _mapper.Map<ProductResponse>(newEntity);
                return await ResponseWrapper<ProductResponse>.SuccessAsync(newModel, "Product created Successfully.");
            }

            return await ResponseWrapper.FailAsync("Failed to create product entry.");
        }
    }
}