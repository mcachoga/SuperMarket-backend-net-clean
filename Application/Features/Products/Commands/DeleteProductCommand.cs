using MediatR;
using SuperMarket.Application.Services;
using SuperMarket.Common.Responses.Wrappers;

namespace SuperMarket.Application.Features.Products.Commands
{
    public class DeleteProductCommand : IRequest<IResponseWrapper>
    {
        public int ProductId { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, IResponseWrapper>
    {
        private readonly IProductService _service;

        public DeleteProductCommandHandler(IProductService service)
        {
            _service = service;
        }

        public async Task<IResponseWrapper> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var currentEntity = await _service.GetProductByIdAsync(request.ProductId);
            
            if (currentEntity is not null)
            {
                var marketId = await _service.DeleteProductAsync(currentEntity);
                return await ResponseWrapper<int>.SuccessAsync(marketId, "Product entry deleted successfully.");
            }
            else
            {
                return await ResponseWrapper.FailAsync("Product does not exist.");
            }
        }
    }
}