using AutoMapper;
using MediatR;
using SuperMarket.Application.Services;
using SuperMarket.Application.Validations.Pipelines;
using SuperMarket.Common.Requests.Products;
using SuperMarket.Common.Responses.Products;
using SuperMarket.Common.Responses.Wrappers;

namespace SuperMarket.Application.Features.Products.Commands
{
    public class UpdateProductCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public UpdateProductRequest UpdateRequest { get; set; }
    }

    public class UUpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, IResponseWrapper>
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;

        public UUpdateProductCommandHandler(IProductService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var currentEntity = await _service.GetProductByIdAsync(request.UpdateRequest.Id);

            if (currentEntity is not null)
            {
                currentEntity.Name = request.UpdateRequest.Name;
                currentEntity.Barcode = request.UpdateRequest.Barcode;
                currentEntity.Description = request.UpdateRequest.Description;

                var updatedEntity = await _service.UpdateProductAsync(currentEntity);
                var model = _mapper.Map<ProductResponse>(updatedEntity);

                return await ResponseWrapper<ProductResponse>.SuccessAsync(model, "Product updated successfully.");
            }

            return await ResponseWrapper<ProductResponse>.FailAsync("Product does not exist.");
        }
    }
}