using AutoMapper;
using MediatR;
using SuperMarket.Application.Services.Contracts;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Infrastructure.Framework.Validations;
using SuperMarket.Shared.Requests.Catalog;
using SuperMarket.Shared.Responses.Catalog;

namespace SuperMarket.Application.Features.Products.Commands
{
    public class UpdateProductCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public UpdateProductRequest UpdateRequest { get; set; }
    }

    public class UUpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, IResponseWrapper>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UUpdateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var currentEntity = await _unitOfWork.Products.GetByIdAsync(request.UpdateRequest.Id);

            if (currentEntity is not null)
            {
                currentEntity.Name = request.UpdateRequest.Name;
                currentEntity.Barcode = request.UpdateRequest.Barcode;
                currentEntity.Description = request.UpdateRequest.Description;

                var updatedEntity = await _unitOfWork.Products.UpdateAsync(currentEntity);
                await _unitOfWork.Commit(cancellationToken);

                var model = _mapper.Map<ProductResponse>(updatedEntity);

                return await ResponseWrapper<ProductResponse>.SuccessAsync(model, "Product updated successfully.");
            }

            return await ResponseWrapper<ProductResponse>.FailAsync("Product does not exist.");
        }
    }
}