using MediatR;
using SuperMarket.Application.Services.Contracts;
using SuperMarket.Infrastructure.Framework.Responses;

namespace SuperMarket.Application.Features.Products.Commands
{
    public class DeleteProductCommand : IRequest<IResponseWrapper>
    {
        public int ProductId { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, IResponseWrapper>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IResponseWrapper> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var currentEntity = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
            
            if (currentEntity is not null)
            {
                var marketId = await _unitOfWork.Products.DeleteAsync(currentEntity);
                await _unitOfWork.Commit(cancellationToken);

                return await ResponseWrapper<int>.SuccessAsync(marketId, "Product entry deleted successfully.");
            }
            else
            {
                return await ResponseWrapper.FailAsync("Product does not exist.");
            }
        }
    }
}