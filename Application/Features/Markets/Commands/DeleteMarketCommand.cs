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
        private readonly IMarketService _service;

        public DeleteMarketCommandHandler(IMarketService service)
        {
            _service = service;
        }

        public async Task<IResponseWrapper> Handle(DeleteMarketCommand request, CancellationToken cancellationToken)
        {
            var currentEntity = await _service.GetMarketByIdAsync(request.MarketId);
            if (currentEntity is not null)
            {
                var marketId = await _service.DeleteMarketAsync(currentEntity);
                return await ResponseWrapper<int>.SuccessAsync(marketId, "Market entry deleted successfully.");
            }
            else
            {
                return await ResponseWrapper.FailAsync("Market does not exist.");
            }
        }
    }
}