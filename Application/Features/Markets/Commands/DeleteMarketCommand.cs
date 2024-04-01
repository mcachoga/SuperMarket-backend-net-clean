using MediatR;
using SuperMarket.Application.Services;
using SuperMarket.Common.Responses.Wrappers;

namespace SuperMarket.Application.Features.Markets.Commands
{
    public class DeleteMarketCommand : IRequest<IResponseWrapper>
    {
        public int MarketId { get; set; }
    }

    public class DeleteMarketCommandHandler : IRequestHandler<DeleteMarketCommand, IResponseWrapper>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteMarketCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IResponseWrapper> Handle(DeleteMarketCommand request, CancellationToken cancellationToken)
        {
            var currentEntity = await _unitOfWork.Markets.GetByIdAsync(request.MarketId);
            if (currentEntity is not null)
            {
                var marketId = await _unitOfWork.Markets.DeleteAsync(currentEntity);
                await _unitOfWork.Commit(cancellationToken);
                return await ResponseWrapper<int>.SuccessAsync(marketId, "Market entry deleted successfully.");
            }
            else
            {
                return await ResponseWrapper.FailAsync("Market does not exist.");
            }
        }
    }
}