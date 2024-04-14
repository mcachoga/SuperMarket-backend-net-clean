using MediatR;
using SuperMarket.Application.Services.Contracts;
using SuperMarket.Infrastructure.Framework.Responses;

namespace SuperMarket.Application.Features.Orders.Commands
{
    public class DeleteOrderCommand : IRequest<IResponseWrapper>
    {
        public int OrderId { get; set; }
    }

    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, IResponseWrapper>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IResponseWrapper> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var currentEntity = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
            
            if (currentEntity is not null)
            {
                var marketId = await _unitOfWork.Orders.DeleteAsync(currentEntity);
                await _unitOfWork.Commit(cancellationToken);

                return await ResponseWrapper<int>.SuccessAsync(marketId, "Order entry deleted successfully.");
            }
            else
            {
                return await ResponseWrapper.FailAsync("Order does not exist.");
            }
        }
    }
}