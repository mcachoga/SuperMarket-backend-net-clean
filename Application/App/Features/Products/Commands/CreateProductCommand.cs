using AutoMapper;
using MediatR;
using SuperMarket.Application.Services.Contracts;
using SuperMarket.Domain;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Infrastructure.Framework.Validations;
using SuperMarket.Shared.Requests.Catalog;
using SuperMarket.Shared.Responses.Catalog;

namespace SuperMarket.Application.Features.Products.Commands
{
    public class CreateProductCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public CreateProductRequest CreateRequest { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, IResponseWrapper>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var model = _mapper.Map<Product>(request.CreateRequest);
            var newEntity = await _unitOfWork.Products.InsertAsync(model);
            await _unitOfWork.Commit(cancellationToken);

            if (newEntity.Id > 0)
            {
                var newModel = _mapper.Map<ProductResponse>(newEntity);
                return await ResponseWrapper<ProductResponse>.SuccessAsync(newModel, "Product created Successfully.");
            }

            return await ResponseWrapper.FailAsync("Failed to create product entry.");
        }
    }
}